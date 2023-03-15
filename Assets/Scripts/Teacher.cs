using UnityEngine;

public class Teacher : InteractablePedestrian
{
    // [SerializeField] private SpawnPoint _signagePt;
    // [SerializeField] private SpawnPoint _signagePtForPlayer;
    // private bool _teacherArrived, _playerArrived, _playerShouldLookAtTeacher;

    // public void WalkOverToSignage()
    // {
    //     _conversation.shouldStop = true;
    //     _playerShouldLookAtTeacher = true;

    //     StartCoroutine(Move(transform.position, _signagePt.point.transform.position, 1.3f, PlayerWalk));
    // }

    // private void PlayerWalk()
    // {
    //     _playerShouldLookAtTeacher = false;
    //     _player.WalkTo(_signagePtForPlayer.point.transform.position, 2.75f, PlayerArrive);
    // }

    // private void PlayerArrive()
    // {
    //     _conversation.shouldStop = false;
    //     StartCoroutine(_LookAt(_player.transform.position, 0.4f));
    //     _player.SetPlayerLookAt(new Vector3(transform.position.x, _player.transform.position.y, transform.position.z), 0.4f);
    // }

    // private void Update()
    // {
    //     if (_playerShouldLookAtTeacher) {
    //         _player.transform.LookAt(new Vector3(transform.position.x, _player.transform.position.y, transform.position.z));
    //     }
    // }
}
