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
        public IPathGroup pathGroup { set; get; }

        /// The transform to use for mapping objects
        private PathTransform objectTransform = new PathTransform();

        /// Parent container; check this to see if the animation should halt
        private IHaltable parent;

        public FollowPathAnimation(IHaltable parent)
        { this.parent = parent; }

        /// Update this animation
        /// @param step The animation step for this update.
        public void AnimationUpdate(GameObject target)
        {
            bool halt = parent.Halted;
            objectTransform.Unity();
            var atarget = AnimationTarget as ISpawnedObjects;
            if (atarget != null)
            {
                bool active = false;
                foreach (var gp in atarget.MetaObjects())
                {
                    // Apply path for specific target
                    foreach (var path in pathGroup.Path)
                    { path.Update(AnimationCurve, objectTransform, gp); }

                    // Check for halt state
                    if ((gp.gameObject.activeSelf) && (!objectTransform.active))
                    {
                        gp.gameObject.transform.name = "Inactive";
                        atarget.Remove(gp.gameObject);
                    }
                    else
                    {
                        // any active objects marker
                        active = true;

                        // Apply to target
                        gp.gameObject.transform.position = objectTransform.position;
                        gp.gameObject.transform.rotation = objectTransform.rotation;
                        gp.gameObject.transform.localScale = objectTransform.scale;

                        // Only update the color if there is one
                        if (gp.renderer != null)
                        { gp.renderer.material.color = objectTransform.color; }
                    }
                }

                // Halt if no active objects
                halt = !active;
            }

            // Stop entire animation
            if (halt)
            { AnimationCurve.Halt(); }
        }
    }
}
