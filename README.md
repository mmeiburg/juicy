# Juicy

As part of my master thesis `Juicy` was created a Unity tool to easily add Juicy to quickly prototype games.

## Motivation

The idea for Juicy came up during my studies when I saw the GDC talk <a href="https://www.youtube.com/watch?v=NU29QKag8a0">Best Practices for fast game design in Unity - Unite LA</a>. There Renaud Forestié described a tool they use internally.
At that time, there was nothing comparable production-ready on the market. However, for our game Wild Woods we needed a good solution to add clean, efficient and extensible effects to the game.

## How it works?

The basics implementation is just a list of `MonoBehaviours` which are hidden on an `GameObject` and styled with some editor magic. Additionaly some effects are done by using <a href="http://dotween.demigiant.com/">DoTween</a> as a tweening libary.
And my own pooling solution <a href="https://github.com/mmeiburg/unityPoolboy">PoolBoy</a>

---

1. To start with you just have to add `JuicyFeedback` as a field to your script.

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

2. If you press the plus button a new feedback list get created

<img src="https://i.imgur.com/FMjuBxJ.png" alt="Feedback List">

3. Now you have a total of 24 effects ordered by category at your disposal
<img src="https://i.imgur.com/YcHwFAO.png" alt="Feedbacks">

4. For example the "Shader" effect. A cool thing about this is that all possible properties of the shader are automatically listed in a dropdown.

<img src="https://i.imgur.com/OvyVoPJ.png" alt="Feedback Shader">

5. If you add a bunch of effect like so, you can get for example the following result
<img src="https://i.imgur.com/2AvKJrZ.png" alt="Example">
<img src="https://i.imgur.com/aKZ9pFP.gif" alt="Example gif">

Or here an example of an `OnHit` feedback in <a href="https://wildwoods.itch.io/wildwoods">Wild Woods</a>

![bnYciEBS0g](https://user-images.githubusercontent.com/46827413/115109240-d98f4500-9f74-11eb-9543-250954730685.gif)

## Conclusion

With the introduction of `[Serialized References]`, one could find a cleaner solution to adding effects than hiding a multitude of `MonoBehaviours`.

As mentioned at the beginning, Renaud Forestié was working on his own tool when I started my work and the idea for the topic of my master thesis.
During the final phase of my thesis he published his tool <a href="https://feedbacks.moremountains.com/">MMFeedbacks</a> in the Unity Asset Store.

## Outlook

At the end of my master's thesis, I came up with an even more designer-friendly way. A Juicy Effect Graph that would consist of fixed assets that could even be reused.
The following is a work in progress prototype of and effect graph build with the Unity Graph API with the help of <a href="https://github.com/alelievr/NodeGraphProcessor">NodeGraphProcessor</a>.

Over easter I had a little time to create this little prototype (not included here).
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






