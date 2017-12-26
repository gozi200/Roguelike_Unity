using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 現在行っているアクションを割り当てる
/// </summary>
public enum Action {
    Invalid               // 0を格納
  , Move                  // 移動処理
  , Attack                // 攻撃処理
  , Get_Item              // 落ちているアイテムを拾う
  , Equip                 // アイテムの装備
  , Drop_Item             // アイテムを捨てる
  , Use_Item              // アイテムを使う
  , Step                  // 階段移動確認
  , Game_Over             // ゲームオーバー
  , Action_Max            // CGameで宣言されているeACTIONの最大値
};

public class Player_Action : MonoBehaviour {
    Action action;
    Direction direction;

    Player player;

    // Use this for initialization
    void Start() {
        action = Action.Move;
    }

    // Update is called once per frame
    void Update() {
        switch (action) {
            case Action.Move:
                Action_Move();
                break;

            case Action.Attack: break;

            case Action.Get_Item: break;

            case Action.Equip: break;

            case Action.Drop_Item: break;

            case Action.Use_Item: break;

            case Action.Step: break;

            case Action.Game_Over: break;
        }
    }

    void Action_Move() {
        // 現在位置をPositionに代入
        Vector2 Position = transform.position;

        // 左キーを押し続けていたら
        if (Input.GetKeyDown("left")) {
            // 代入したPositionに対して加算減算を行う
            Position.x -= player.SPEED.x;
            direction = Direction.Left;
        }
        else if (Input.GetKeyDown("right")) { // 右キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.x += player.SPEED.x;
            direction = Direction.Right;
        }
        else if (Input.GetKeyDown("up")) { // 上キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.y += player.SPEED.y;
            direction = Direction.Up;
        }
        else if (Input.GetKeyDown("down")) { // 下キーを押し続けていたら
            // 代入したPositionに対して加算減算を行う
            Position.y -= player.SPEED.y;
            direction = Direction.Down;
        }

        // 現在の位置に加算減算を行ったPositionを代入する
        transform.position = Position;
    }
}
