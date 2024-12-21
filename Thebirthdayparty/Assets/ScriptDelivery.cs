using System.Collections;
using System.Collections.Generic;
using System.Net;
using Interactable;
using QFramework;
using TMPro;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;
namespace Delivery
{
    public class ScriptDelivery : MonoBehaviour
    {
        class Node
        {
            public bool isDone = false;
            public string[] content;
            public Node(string[] arg)
            {
                content = arg;
            }
        }
        public bool isAbleToMove = false;
        string choice = "";
        public GameObject Buttons;
        Node deliverman = new(new string[]{
        "Courier: Hello, there's a package for you.",
        "Courier: Need your signature."
        });
        Node openTheDoor = new(new string[]{
        "Courier: Thanks, here's the package...",
        "Courier: Signature... here."
        });
        Node askTheMan = new(new string[]{
        "Courier: It's from Mr. George.",
        "Courier: He specifically requested delivery.",
        "Hearing that is George, you think its better to take a look."
        });
        Node theEquipment = new(new string[]{
            "Now, here's the device in front of you.",
            "This is a retro-styled terminal device",
            "The device has black casing, with some wear",
            "It appears to be quite old..."
        });
        Node theNote = new(new string[]{
            "There's a note beside the device",
            "Note reads:",
            "\"pw: Its will, your choice.\"",
            "What does that mean?",
            "You feel it's better to call George",
            "But there was no answer on the other end of the line."
        });
        Node checkIt = new(new string[]{
            "You decide to give it a check."
        });


        private void Start()
        {
            TypeEventSystem.Global.Register<InteractChoiceGotoEvent>(e =>
            {
                choice = e.Name;
                Debug.Log("GET " + choice);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            TypeEventSystem.Global.Register<InteractTextParagraphQuited>(e =>
            {
                isAbleToMove = true;
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            Buttons = Instantiate(Buttons, InteractPanel.Instance.textPrinter.transform);
            Buttons.SetActive(false);
            StartCoroutine(IERunning());
        }
        IEnumerator IERunning()
        {
            TypeEventSystem.Global.Send<InteractStart>();
            Buttons.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isAbleToMove = false;
            choice = "";
            InteractPanel.Instance.Show(deliverman.content, true);
            yield return new WaitUntil(() => isAbleToMove);
            SetButton("Open the door", "\"I didn't order anything!\"");
            yield return new WaitUntil(() => !choice.Equals(""));


            if (choice.Equals("Open the door"))
            {
                isAbleToMove = false;
                choice = "";
                InteractPanel.Instance.Show(openTheDoor.content, true);
            }
            else
            {
                isAbleToMove = false;
                choice = "";
                InteractPanel.Instance.Show(askTheMan.content, true);
            }
            yield return new WaitUntil(() => isAbleToMove);


            isAbleToMove = false;
            choice = "";
            InteractPanel.Instance.Show(theEquipment.content, true);
            yield return new WaitUntil(() => isAbleToMove);
            while (!(checkIt.isDone && theNote.isDone))
            {
                SetButton(checkIt.isDone ? "" : "Check the device", theNote.isDone ? "" : "Check the note");
                yield return new WaitUntil(() => !choice.Equals(""));
                if (choice.Equals("Check the device"))
                {
                    isAbleToMove = false;
                    choice = "";
                    checkIt.isDone = true;
                    InteractPanel.Instance.Show(checkIt.content, true);
                    yield return new WaitUntil(() => isAbleToMove);
                    var power = new Node(new string[]{
                        "You try to find a power button on the device, but you find nothing.",
                        "Some how, you feel familiar with this",
                        "Blurry images flash by...",
                        "A familiar place, three people sitting together..."
                    });
                    var back = new Node(new string[]{
                        "There's only a fading marking on it's back, reads:",
                        "Terminal inc."
                    });
                    var handle = new Node(new string[]{
                        "It's funny.",
                        "But nothing happened."
                    });
                    while (!(power.isDone && back.isDone && handle.isDone))
                    {
                        isAbleToMove = false;
                        choice = "";
                        SetButton(power.isDone ? "" : "Look for power button",
                        back.isDone ? "" : "Check device's back",
                        handle.isDone ? "" : "Handle it");
                        yield return new WaitUntil(() => !choice.Equals(""));
                        if (choice.Equals("Look for power button"))
                        {
                            power.isDone = true;
                            InteractPanel.Instance.Show(power.content, true);
                        }
                        else if (choice.Equals("Check device's back"))
                        {
                            back.isDone = true;
                            InteractPanel.Instance.Show(back.content, true);
                        }
                        else
                        {
                            handle.isDone = true;
                            InteractPanel.Instance.Show(handle.content, true);
                        }
                        yield return new WaitUntil(() => isAbleToMove);

                    }
                    choice = "";
                }
                else
                {
                    isAbleToMove = false;
                    choice = "";
                    theNote.isDone = true;
                    InteractPanel.Instance.Show(theNote.content, true);
                }
                yield return new WaitUntil(() => isAbleToMove);
            }
            yield return new WaitUntil(() => isAbleToMove);
            isAbleToMove = false;
            choice = "";
            InteractPanel.Instance.Show(new string[]{
                "\"Well.\"",
                "Now what should you do to the device?"
            }, true);
            yield return new WaitUntil(() => isAbleToMove);
            isAbleToMove = false;
            choice = "";
            SetButton("Try to start the device");
            yield return new WaitUntil(() => !choice.Equals(""));
            InteractPanel.Instance.Show(new string[]{
                "But you haven't even been able to plug it in yet!",
                "How would you"
            }, true);
            yield return new WaitUntil(() => isAbleToMove);
            isAbleToMove = false;
            choice = "";
            SetButton("Try to start the device");
            yield return new WaitUntil(() => !choice.Equals(""));
            InteractPanel.Instance.Show(new string[]{
                "The screen lit up",
                "Amazing.",
                "Screen flickers dimly...",
                "Showing login interface...",
                "Password required..."
            }, true);
            yield return new WaitUntil(() => isAbleToMove);
            isAbleToMove = false;
            choice = "";
            SetButton("\"Its will, your choice.\"");
            yield return new WaitUntil(() => !choice.Equals(""));
            InteractPanel.Instance.Show(new string[]{
                "You are logging in",
                "\"Unable to log in, the user has been online or there is a repeated start of the terminal, please close the system and try again.\"",
                "That's sad.",
                "Anyway, you decide to keep it, maybe take it to the party."
            }, true);
            yield return new WaitUntil(() => isAbleToMove);
            TypeEventSystem.Global.Send<NextPeriodEvent>();
            TypeEventSystem.Global.Send<RequireSaveGame>();
            TypeEventSystem.Global.Send<InteractTextQuited>();
            TypeEventSystem.Global.Send<InteractEnd>();
        }
        void SetButton(string a = "", string b = "", string c = "")
        {
            Buttons.SetActive(true);
            var A = Buttons.transform.Find("Button1");
            var B = Buttons.transform.Find("Button2");
            var C = Buttons.transform.Find("Button3");
            Transform[] gameObjects = new[] { A, B, C };
            string[] strings = new[] { a, b, c };
            int i = 0;
            if (strings[i].IsNotNullAndEmpty())
            {
                var temp = gameObjects[i].GetComponent<Button>();
                temp.enabled = true;
                temp.interactable = true;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("> " + strings[i]);
                temp.onClick.RemoveAllListeners();
                temp.onClick.AddListener(() =>
                {
                    TypeEventSystem.Global.Send(new InteractChoiceGotoEvent(a));
                    Buttons.SetActive(false);
                });
            }
            else
            {
                gameObjects[i].GetComponent<Button>().interactable = false;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("");
            }
            i = 1;
            if (strings[i].IsNotNullAndEmpty())
            {
                var temp = gameObjects[i].GetComponent<Button>();
                temp.enabled = true;
                temp.interactable = true;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("> " + strings[i]);
                temp.onClick.RemoveAllListeners();
                temp.onClick.AddListener(() =>
                {
                    TypeEventSystem.Global.Send(new InteractChoiceGotoEvent(b));
                    Buttons.SetActive(false);
                });
            }
            else
            {
                gameObjects[i].GetComponent<Button>().interactable = false;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("");
            }
            i = 2;
            if (strings[i].IsNotNullAndEmpty())
            {
                var temp = gameObjects[i].GetComponent<Button>();
                temp.enabled = true;
                temp.interactable = true;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("> " + strings[i]);
                temp.onClick.RemoveAllListeners();
                temp.onClick.AddListener(() =>
                {
                    TypeEventSystem.Global.Send(new InteractChoiceGotoEvent(c));
                    Buttons.SetActive(false);
                });
            }
            else
            {
                gameObjects[i].GetComponent<Button>().interactable = false;
                gameObjects[i].GetComponentInChildren<TextMeshProUGUI>().SetText("");
            }
        }
    }
    public class InteractChoiceGotoEvent
    {
        public string Name;
        public InteractChoiceGotoEvent(string Name)
        {
            this.Name = Name;
        }
    }
}
