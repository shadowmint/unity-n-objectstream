using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
  /// The animation that is invoked on objects during the stream
  public class FollowPathAnimation : IAnimation
  {
    /// The targets for this animation
    public IAnimationTarget AnimationTarget { set; get; }

    /// The animation curve for this animation
    public IAnimationCurve AnimationCurve { set; get; }

    /// The object stream to apply
    public IPathGroup PathGroup { set; get; }

    /// The transform to use for mapping objects
    private readonly PathTransform _objectTransform = new PathTransform();

    /// Parent container; check this to see if the animation should halt
    private readonly IHaltable _parent;

    public FollowPathAnimation(IHaltable parent)
    {
      _parent = parent;
    }

    /// Update this animation
    /// @param step The animation step for this update.
    public void AnimationUpdate(GameObject target)
    {
      var halt = _parent.Halted;
      _objectTransform.Unity();
      try
      {
        var atarget = AnimationTarget as ISpawnedObjects;
        if (atarget != null)
        {
          var active = false;
          foreach (var gp in atarget.MetaObjects())
          {
            // Apply path for specific target
            foreach (var path in PathGroup.Path)
            {
              path.Update(AnimationCurve, _objectTransform, gp);
            }

            // Check for halt state
            if ((gp.GameObject.activeSelf) && (!_objectTransform.Active))
            {
              gp.GameObject.transform.name = "Inactive";
              atarget.Remove(gp.GameObject);
            }
            else
            {
              // any active objects marker
              active = true;

              // Apply to target
              gp.GameObject.transform.position = _objectTransform.Position;
              gp.GameObject.transform.rotation = _objectTransform.Rotation;
              gp.GameObject.transform.localScale = _objectTransform.Scale;

              // Only update the color if there is one
              if (gp.Renderer == null) continue;
              if (gp.Renderer.material == null) continue;
              var c = gp.Renderer.material.color;
              c.a = ((Color) _objectTransform.Color).a;
              gp.Renderer.material.color = c;
            }
          }

          // Halt if no active objects
          halt = !active;
        }
      }
      catch (MissingReferenceException)
      {
        // Halt animation on broken objects.
      }

      // Stop entire animation
      if (halt)
      {
        AnimationCurve.Halt();
      }
    }
  }
}