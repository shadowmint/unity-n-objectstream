using UnityEngine;
using N.Package.Animation;

namespace N.Package.ObjectStream
{
  /// Use this to halt Animations in progress without disrupting the
  /// animation manager.
  public interface IHaltable
  {
    /// Stop now
    void Halt();

    /// Resume
    void Resume();

    /// Has this target halted?
    bool Halted { get; }
  }
}