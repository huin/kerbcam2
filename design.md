## Concepts for Kerbcam2

### Story

Top-level container object for a timeline, its tracks, and operations.

### Timeline

Contains a set of points in time relative to a "time zero" start point. These
points in time *may* be named.

### Time key

(Could there be a better name?)

A point in time (within a timeline) that may be referenced by operations. It
may or may not be named for convenience.

### Track

Not a functionally required concept, but potentially useful for the user to
organise operations into groups within a timeline purely to serve a
user-interface need to present and organise a sequence of operations clearly. A
constraint being that operations that overlap in time cannot exist within the
same track. An operation exists in exactly one track (maybe zero tracks in a
transitional state if there are copy/paste mechanics).

A track may or may not be named for convenience.

### Operation

Something that manipulates the camera (or potentially part of the scene) as
time progresses. This uses points in time referenced from the timeline. An
operation may be instantaneous, or cover a range of times.

Examples of operations:
* Interpolate camera zoom (field-of-view).
* Interpolate camera position.
* Interpolate camera look-at point.
* Invoke an action on a part (e.g activate an engine).
* (Maybe) Force GC cycle for perfectionists - maybe this point in the sequence
  isn't too (subjectively) sensitive to GC pauses, but a part that *is*
  sensitive is coming up.
* (Maybe) Set camera visible layers.

Operations *might* have sub-operations in future, but that's just a
speculative feature idea for now.

### Actuator

A general set of interfaces for something that acts as a proxy for modifying
the camera or scene. This is distinct from modifying the object(s) directly so
that the actuator can:

* Cleanup after playback.
* Reduce the number of places that Kerbcam interfaces with KSP's own API, thus
  limiting the impact of breaking changes in KSP.
* Be used as a stub to create preview visualizations. (E.g a stub object that
  implements a camera actuator can record camera position etc. over time and
  then be used to render an object showing how the camera moves)
* Potentially be used as a fake for unit testing actuators etc.
* (Maybe - future speculation) Further use in layering operations. E.g a
  camera looking direction operation transitioning to another by a
  interpolation might delegate to both looking-direction sub-operations and
  smoothly interpolate between the results on stubbed cameras and apply that to
  another actuator that applies to the actual camera.

Actuators should exist to support for the various types of operation that
exist.

## Misc thoughts

Time.captureFramerate - potentially useful to set a framerate if we're going
to capture at a given rate, might make the framerate more predictable.

Create lights to move around a scene as an operation.

## User Interface

Mockups made using [ASCIIFlow](http://asciiflow.com/).

Open questions:
* How should things be grouped into windows? For example, might the "main
  window" be arranged into 3 sections:
  * Playback controls.
  * Story overview.
  * Item properties (for editing an item selected in the story overview).

### Playback controls

* Simple "Play" toggle.
* Auto-play setting that causes playback (i.e time "zero" on a timeline) to
  begin at a given UT (game universe time). This UT time setting maybe should
  be part of the data for the story.

### Story overview

This mockup shows how the timeline UI might look with a reasonably simple
story.


              +-------+-------+-------+---------------+--------+--------+
              | 0.00" | 3.15" | 7.50" | 10.23"        | 12.73" | 20.28" |
              | Start |       |       | Engines start |        |        |
    +-----------------+---------------+---------------+--------+--------+
    |         |               |                                         |
    | Track 1 | Camera pos... | Camera position                         |
    |         |               |                                         |
    +-------------------------------------------------------------------+
    |         |               |                                         |
    | Track 2 | Look at po... | Look at position                        |
    |         |               |                                         |
    +-------------------------+-------+---------------+-----------------+
    |         |                       |               |
    | Track 3 |                       | Invoke        |
    |         |                       |               |
    +---------+                       +---------------+

Discussion of each component in this mockup:
* The top row is the timeline, containing the time keys. A couple of them are
  named, but they all exist at specific times, and are kept in order of
  increasing time from left to right.
* Each row below that is a track, in this example they just have default names.
* Each track row contains operations. In this example:
 * "Track 1" contains operations that move the camera, the first operation is a
   movement of the camera position starting at 0.00", and finishing at 7.50".
   Immediately following that, the camera starts from a new position at 7.50",
   and continues moving until 20.28".
 * "Track 2" is similar to "Track 1" but contains camera "look-at" operations
   instead of camera positioning.
 * "Track 3" contains a single operation at 10.23". This operation (like the
   others) has a default name. It has been configured to invoke the "activate"
   action on some rocket engines on a vessel.

UI decisions made by this mockup (not necessarily set in stone):
* Things can optionally be named (specifically "time keys", "tracks" and
  "operations").
* The column widths are determined by the time key names, but not by the
  operation names. Not clear if this is a good idea or not, or if it's
  practical within Unity3d.

### Path editing

When editing/viewing paths (particularly camera positions, look-at positions),
the path should be visible by the flight camera, so its position can be seen
relative to the environment/vessel. The nodes should also be visible (and
ideally clickable to select). The position of each node should be adjustable
via controls in a window. A node's timing is determined by a referenced time
key. Minimal controls would include X,Y,Z adjusters.

Ideally the position of each node would be adjustable via something akin to the
manuever node interface that KSP players are familiar with. While this seems to
be possible, that's regarded as a stretch/later feature (not a high priority).

A node's position (or the entire path) should be relative to a reference frame.
Examples of reference frames:

* Selected vessel/part/object
 * including rotation
 * ignoring rotation
 * reference position relative to object's position, plus some multiple of the
   object's velocity vector relative to the SOI (rotating/non-rotating SOI).
   Questions remain open as to when the position and velocity are sampled. The
   intent is to help with fly-by effects, but some clarity is needed here.
