# Juicy

As part of my master thesis I created `Juicy` a Unity tool to easily add juice to quickly prototype games.

## Motivation

The idea for `Juicy` came up during my studies when I saw the GDC talk <a href="https://www.youtube.com/watch?v=NU29QKag8a0">Best Practices for fast game design in Unity - Unite LA</a>.
There Renaud Forestié described a tool they use internally.
At that time, there was nothing comparable production-ready on the market. However, for our game <a href="https://wildwoods.itch.io/wildwoods">Wild Woods</a> we needed a good solution to add clean, efficient and extensible effects to the game.

## How it works?

The basics implementation is just a list of `MonoBehaviours` which are hidden on a `GameObject` and styled with some editor magic. Additionaly some effects are done by using <a href="http://dotween.demigiant.com/">DoTween</a> as a tweening libary and my own pooling solution <a href="https://github.com/mmeiburg/unityPoolboy">PoolBoy</a>.

---

To start with you just have to add `JuicyFeedback` as a field to your script.

```cs
    public class JuicyFeedbackTest : MonoBehaviour
    {
        [SerializeField] private JuicyFeedback feedback;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) {
                feedback.Play();
            }
        }
    }
```
<img src="https://i.imgur.com/hl4OovH.png" alt="Feedback">

---

If you press the plus button a new feedback list will be created.

<img src="https://i.imgur.com/FMjuBxJ.png" alt="Feedback List">

Now you have a total of 24 <a href="https://github.com/mmeiburg/juicy/blob/main/README.md#effect-list">effects</a> ordered by category at your disposal
<img src="https://i.imgur.com/YcHwFAO.png" alt="Feedbacks">

---

Here for example the "Shader" effect. A cool thing about this is that all possible properties of the shader are automatically listed in a dropdown.

<img src="https://i.imgur.com/OvyVoPJ.png" alt="Feedback Shader">

---

If you add a bunch of effect like so, you get the following result

<table style="width:100%">
  <tr>
    <td><img src="https://i.imgur.com/2AvKJrZ.png" alt="Example"></td>
    <td><img src="https://i.imgur.com/aKZ9pFP.gif" alt="Example gif"></td>
  </tr>
</table>

---

Or here an example of an `OnHit` feedback in <a href="https://wildwoods.itch.io/wildwoods">Wild Woods</a>. Here you can also see nicely the grass cutting particle effect created with the same system.

![bnYciEBS0g](https://user-images.githubusercontent.com/46827413/115109240-d98f4500-9f74-11eb-9543-250954730685.gif)

But the best thing about this plugin is, that you can easily extent the system by creating your own effects. Just create a new class and inherit from `JuicyFeedbackBase` with the `[Feedback]` attribute that way you can change the menu path and name. Like in the following example:

```cs
    [Feedback("Debug/Log")]
    public class DebugLogFeedback : JuicyFeedbackBase
    {
        [SerializeField] private LogType logType = LogType.Log;
        [SerializeField] private string message;
                
        protected override void Play()
        {
            Debug.unityLogger.Log(logType, message);
        }
    }
```

## Effect List

Almost every effect comes with timing values, like duration, delay and cooldown to change values over time and with an ease.

| Effect        | Description   |
| ------------- |:-------------:|
| Animator      | Change animator values (trigger, float ...) |
| Audio Oneshoot  | Instantiates a oneshoot audio |
| Audio Pooled | Instantiates a pooled audio |
| Camera Shake | Shakes the camera |
| Camera Zoom | Change camera FOV |
| Event Simple| A single Unity Event |
| Event Lifetime | Unity Events for the Unity lifecyles (Awake, Start ...)    |
| Light Color | Change the light color |
| Light Intensity | Change the light intensity |
| Object Create | Creates an object from a prefab  |
| Object Fade | Changes the alpha value of an renderer |
| Object Move | Moves an object  |
| Object Punch | Punch an object scale to a value and back |
| Object Rotate | Rotates an object  |
| Object Scale | Scales an object |
| Object Shake | Changes objects rotation to a value and back |
| Object Tint | Tint the objects material base color  |
| Particle Create | Instantiates a particle from a prefab  |
| Particle Pooled | Instantiates a pooled particle from a prefab  |
| Screen Flash | Overlays the screen with a color  |
| Shader | Effect to change shader properties  |
| Time Change | Change the time to a value and back  |
| Time Freeze | Freeze the time |

## Conclusion

With the introduction of the `[SerializeReference]` attribute which allows you do serialize interfaces and handle inheritance, one could find a better solution to add effects than without hiding a multitude of `MonoBehaviours`.

As mentioned at the beginning, Renaud Forestié was working on his own tool when I started my work and the idea for the topic of my master thesis.
During the final phase of my thesis he published his tool <a href="https://feedbacks.moremountains.com/">MMFeedbacks</a> in the Unity Asset Store.

## Outlook

At the end of my master's thesis, I came up with an even more designer-friendly way. A Juicy Effect Graph that would consist of fixed assets that could even be reused.
The following is a work in progress prototype of and effect graph build with the Unity Graph API with the help of <a href="https://github.com/alelievr/NodeGraphProcessor">NodeGraphProcessor</a>.

Over easter holidays I had a little time to create this little prototype (not included here).
The idea is, that you can add parameters to a graph at runtime. Here for example on the right side with the `Juice` component. You can see the component has a reference to a graph asset.
A editor script finds all possible properties and serializes the entered parameters.
<img src="https://i.imgur.com/H3qzzyN.png" alt="Feedback List">

## Made with

<a href="https://wildwoods.itch.io/wildwoods">
<img src="https://img.itch.zone/aW1nLzIyNzAzMjUucG5n/315x250%23c/j71zvH.png" alt="Tangle Toys" width="200" height="150">
</a>
<a href="https://1-jar.itch.io/cuddle-waddle">
<img src="https://img.itch.zone/aW1nLzMxOTk1MTgucG5n/315x250%23c/8bgbl7.png" alt="Cuddle Waddle" width="200" height="150">
</a>
