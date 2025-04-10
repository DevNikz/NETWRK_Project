using Unity.Netcode;
using UnityEngine;
public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private bool _usingServerAuth;
    [SerializeField] private float _cheapInterpolationTime = 0.1f;

    private NetworkVariable<PlayerNetworkState> _playerState;
    private Rigidbody2D _rb;
    private PlayerUI playerHUD;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        playerHUD = GetComponent<PlayerUI>();

        var permission = _usingServerAuth ? NetworkVariableWritePermission.Server : NetworkVariableWritePermission.Owner;
        _playerState = new NetworkVariable<PlayerNetworkState>(writePerm: permission);
    }

    public override void OnNetworkSpawn()
    {
        SessionController.instance.AddPlayer(this.gameObject);

        if(!IsOwner) {
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerShooter>().enabled = false;
            GetComponent<PlayerUI>().enabled = false;
        }
    }

    void Update()
    {
        if(IsOwner) TransmitState(); 
        else ConsumeState();
    }

    private void TransmitState() {
        var state = new PlayerNetworkState {
            Position = _rb.position,
        };

        if(IsServer || !_usingServerAuth) {
            _playerState.Value = state;
        }

        else {
            TransmitStateServerRpc(state);
        }
    }

    private Vector2 _posVel;

    private void ConsumeState() {
        _rb.MovePosition(Vector2.SmoothDamp(transform.position, 
                        _playerState.Value.Position, ref _posVel, _cheapInterpolationTime));
    }

    [ServerRpc]
    private void TransmitStateServerRpc(PlayerNetworkState state) {
        _playerState.Value = state;
    }

    struct PlayerNetworkState : INetworkSerializable {
        private float _x, _y;

        internal Vector2 Position {
            get => new Vector2(_x, _y);
            set {
                _x = value.x;
                _y = value.y;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
        }
    }
}
