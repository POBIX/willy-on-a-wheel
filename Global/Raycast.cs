using Godot;
using Godot.Collections;

public struct RaycastHit
{
    public Vector2 point;
    public Vector2 normal;
    public Node2D collider;
}

public class Raycast : Node2D
{
    public static Raycast Ref { get; private set; }

    public Raycast() => Ref = this;

    public static bool Intersects(Vector2 from, Vector2 to, out RaycastHit hit, uint layer, params object[] exclude)
    {
        hit = new();
        Dictionary d = Ref.GetWorld2d().DirectSpaceState.IntersectRay(from, to, new Array(exclude), layer);
        if (d.Count != 0)
        {
            hit.point = (Vector2)d["position"];
            hit.normal = (Vector2)d["normal"];
            hit.collider = (Node2D)d["collider"];
            return true;
        }
        return false;
    }
}
