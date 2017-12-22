using UnityEngine;
using System.Collections;

/// キャラクター基底クラス
public class Token : MonoBehaviour {

	/// プレハブ取得
	/// プレハブは必ず"Resources/Prefabs/"に配置すること
	public static GameObject Get__Position_Prefab(GameObject prefab, string name) {
		return prefab ?? (prefab = Resources.Load("Prefabs/"+name) as GameObject);
	}

	/// インスタンスを生成してスクリプトを返す
	public static Type Create_Instance<Type>(GameObject prefab, Vector3 p, float direction=0.0f, float speed=0.0f) where Type : Token {
		GameObject g = Instantiate(prefab, p, Quaternion.identity) as GameObject;
		Type obj = g.GetComponent<Type>();
		obj.Set__Position_Velocity(direction, speed);
		return obj;
	}
	public static Type Create_Instance2<Type>(GameObject prefab, float x, float y, float direction=0.0f, float speed=0.0f) where Type : Token {
		Vector3 pos = new Vector3(x, y, 0);
		return Create_Instance<Type>(prefab, pos, direction, speed);
	}
	/// 生存フラグ
	bool _exists = false;
	public bool Exists {
		get { return _exists; }
		set { _exists = value; }
	}

	/// アクセサ
	/// レンダラー
	SpriteRenderer _renderer = null;
	public SpriteRenderer Renderer
	{
		get { return _renderer ?? (_renderer = gameObject.GetComponent<SpriteRenderer>()); }
	}
	/// 描画フラグ
	public bool Visible
	{
		get { return Renderer.enabled; }
		set { Renderer.enabled = value; }
	}
	/// ソーティングレイヤー名
	public string Sorting_Layer
	{
		get { return Renderer.sortingLayerName; }
		set { Renderer.sortingLayerName = value; }
	}
	/// ソーティング・オーダー
	public int Sorting_Order
	{
		get { return Renderer.sortingOrder; }
		set { Renderer.sortingOrder = value; }
	}
	/// 座標(X)
	public float X
	{
		set { Vector3 pos = transform.position; pos.x = value; transform.position = pos; }
		get { return transform.position.x; }
	}
	/// 座標(Y)
	public float Y
	{
		set { Vector3 pos = transform.position; pos.y = value; transform.position = pos; }
		get { return transform.position.y; }
	}
	/// 座標を足し込む
	public void Add_Position(float dx, float dy) {
		X += dx;
		Y += dy;
	}
	/// 座標を設定する
	public void Set_Position(float x, float y) {
		Vector3 pos = transform.position;
		pos.Set(x, y, 0);
		transform.position = pos;
	}
	/// スケール値(X)
	public float ScaleX
	{
		set { Vector3 scale = transform.localScale; scale.x = value; transform.localScale = scale; }
		get { return transform.localScale.x; }
	}
	/// スケール値(Y)
	public float ScaleY
	{
		set { Vector3 scale = transform.localScale; scale.y = value; transform.localScale = scale; }
		get { return transform.localScale.y; }
	}
	/// スケール値を設定
	public void Set__PositionScale(float x, float y) {
		Vector3 scale = transform.localScale;
		scale.Set(x, y, (x+y)/2);
		transform.localScale = scale;
	}
	/// スケール値(X/Y)
	public float Scale
	{
		get { Vector3 scale = transform.localScale; return (scale.x + scale.y) / 2.0f; }
		set { Vector3 scale = transform.localScale; scale.x = value; scale.y = value; transform.localScale = scale; }
	}
	/// スケール値を足し込む
	public void Add_Scale(float d) {
		Vector3 scale = transform.localScale;
		scale.x += d;
		scale.y += d;
		transform.localScale = scale;
	}
	/// スケール値をかける
	public void Mul_Scale(float d) {
		transform.localScale *= d;
	}
	/// 剛体
	Rigidbody2D rigidbody2D = null;
	public Rigidbody2D RigidBody
	{
		get { return rigidbody2D ?? (rigidbody2D = gameObject.GetComponent<Rigidbody2D>()); }
	}
	/// 移動量を設定
	public void Set__Position_Velocity(float direction, float speed) {
		Vector2 v;
		v.x = Util.Cos_Ex(direction) * speed;
		v.y = Util.Sin_Ex(direction) * speed;
		RigidBody.velocity = v;
	}
	/// 移動量を設定(X/Y)
	public void Set__Position_Velocity_X_Y(float vx, float vy) {
		Vector2 v;
		v.x = vx;
		v.y = vy;
		RigidBody.velocity = v;
	}
	/// 移動量をかける
	public void Mul_Velocity(float d) {
		RigidBody.velocity *= d;
	}
	/// 移動量(X)
	public float VX
	{
		get { return RigidBody.velocity.x; }
		set { Vector2 v = RigidBody.velocity; v.x = value; RigidBody.velocity = v; }
	}
	/// 移動量(Y)
	public float VY
	{
		get { return RigidBody.velocity.y; }
		set { Vector2 v = RigidBody.velocity; v.y = value; RigidBody.velocity = v; }
	}
	/// 方向
	public float Direction
	{
		get { Vector2 v = GetComponent<Rigidbody2D>().velocity; return Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg; }
	}
	/// 速度
	public float Speed
	{
		get { Vector2 v = GetComponent<Rigidbody2D>().velocity; return Mathf.Sqrt(v.x*v.x + v.y*v.y); }
	}
	/// 重力
	public float Gravity_Scale
	{
		get { return RigidBody.gravityScale; }
		set { RigidBody.gravityScale = value; }
	}
	/// 回転角度
	public float Angle
	{
		set { transform.eulerAngles = new Vector3(0, 0, value); }
		get { return transform.eulerAngles.z; }
	}
	/// スプライトの設定
	public void Set_Position_Sprite(Sprite sprite) {
		Renderer.sprite = sprite;
	}
	/// 色設定
	public void Set__Position_Color(float r, float g, float b) {
		var c = Renderer.color;
		c.r = r; c.g = g; c.b = b;
		Renderer.color = c;
	}
	/// アルファ値を設定
	public void Set__Position_Alpha(float a) {
		var c = Renderer.color;
		c.a = a;
		Renderer.color = c;
	}
	/// アルファ値を取得
	public float Get__Position_Alpha() {
		var c = Renderer.color;
		return c.a;
	}
	/// アルファ値
	public float Alpha
	{
		set { Set__Position_Alpha(value); }
		get { return Get__Position_Alpha(); }
	}
	/// サイズを設定
	float _width = 0.0f;
	float _height = 0.0f;
	public void Set__PositionSize(float width, float height) {
		_width = width;
		_height = height;
	}
	public float Sprite_Width {
		get { return Renderer.bounds.size.x; }
	}
	public float Sprite_Height {
		get { return Renderer.bounds.size.y; }
	}
  /// コリジョン（円）
	CircleCollider2D circle_Collider = null;
	public CircleCollider2D Circle_Collider {
		get { return circle_Collider ?? (circle_Collider = GetComponent<CircleCollider2D>()); }
	}
  // 円コリジョンの半径
	public float Collision_Radius {
		get { return Circle_Collider.radius; }
		set { Circle_Collider.radius = value; }
	}
  // 円コリジョンの有効無効を設定する
  public bool Circle_Collider_Enabled {
		get { return Circle_Collider.enabled; }
		set { Circle_Collider.enabled = value; }
	}
  /// コリジョン（矩形）
  BoxCollider2D _boxCollider = null;
  public BoxCollider2D Box_Collider {
    get { return _boxCollider ?? (_boxCollider = GetComponent<BoxCollider2D>()); }
  }
  public float BoxCollider_Width {
    get { return Box_Collider.size.x; }
    set {
      var size = Box_Collider.size;
      size.x = value;
      Box_Collider.size = size;
    }
  }
  public float BoxColliderHeight {
    get { return Box_Collider.size.y; }
    set {           
      var size = Box_Collider.size;
      size.y = value;
      Box_Collider.size = size;
    }
  }
  // 箱コリジョンのサイズを設定する
  public void Set__PositionBoxColliderSize(float w, float h) {
    Box_Collider.size.Set(w, h);
  }
  // 箱コリジョンの有効無効を設定する
  public bool BoxColliderEnabled {
    get { return Box_Collider.enabled; }
    set { Box_Collider.enabled = value; }
  }

	/// 移動して画面内に収めるようにする
	public void ClampScreenAndMove(Vector2 v) {
		Vector2 min = Get__Position_World_Min();
		Vector2 max = Get__Position_World_Max();
		Vector2 pos = transform.position;
		pos += v;

		// 画面内に収まるように制限をかける
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		// プレイヤーの座標を反映
		transform.position = pos;
	}
	/// 画面内に収めるようにする
	public void ClampScreen() {
		Vector2 min = Get__Position_World_Min();
		Vector2 max = Get__Position_World_Max();
		Vector2 pos = transform.position;
		// 画面内に収まるように制限をかける
		pos.x = Mathf.Clamp(pos.x, min.x, max.x);
		pos.y = Mathf.Clamp(pos.y, min.y, max.y);

		// プレイヤーの座標を反映
		transform.position = pos;
	}

	/// 画面外に出たかどうか
	public bool IsOutside() {
		Vector2 min = Get__Position_World_Min();
		Vector2 max = Get__Position_World_Max();
		Vector2 pos = transform.position;
		if(pos.x < min.x || pos.y < min.y) {
			return true;
		}
		if(pos.x > max.x || pos.y > max.y) {
			return true;
		}
		return false;
	}

	/// 画面の左下のワールド座標を取得する
	public Vector2 Get__Position_World_Min(bool noMergin=false) {
		Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
		if(noMergin) {
			// そのまま返す
			return min;
		}

		// 自身のサイズを考慮する
		min.x += _width;
		min.y += _height;
		return min;
	}
	/// 画面右上のワールド座標を取得する
	public Vector2 Get__Position_World_Max(bool noMergin=false) {
		Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
		if(noMergin) {
			// そのまま返す
			return max;
		}

		// 自身のサイズを考慮する
		max.x -= _width;
		max.y -= _height;
		return max;
	}

	/// 消滅（メモリから削除）
	public void Destroy_Obj() {
		Destroy(gameObject);
	}
	/// アクティブにする
	public void Revive() {
		gameObject.SetActive(true);
		Exists = true;
		Visible = true;
	}
	/// 消滅する
	public void Vanish() {
		gameObject.SetActive(false);
		Exists = false;
	}
}
