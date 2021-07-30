using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    private Queue<string> sentences;
    private Queue<string> sentencesCopy;
    public bool conversationStarted;
    public Animator animator;
    private Dialogue dialogue1;
    
    // Start is called before the first frame update
    void Start()
    {
        conversationStarted = false;
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogue1 = dialogue;
        sentencesCopy = sentences;
        print("started");
        animator.SetBool("isOpen",true);
        print(animator.GetBool("isOpen"));
        conversationStarted = true;
        //Debug.Log("Starting conversation with " + dialogue.name);
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));

    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void EndDialogue()
    {
        foreach (string sentence in dialogue1.sentences)
        {
            sentences.Enqueue(sentence);
        }
        animator.SetBool("isOpen",false);
        conversationStarted = false;
        Debug.Log("end");
    }
    // Update is called once per frame
    
}
