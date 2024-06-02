using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : SingletonMonoBehavior<DialogueManager>
{
    public Image actorImage;
    public TextMeshProUGUI actorName;
    public TextMeshProUGUI messageText;
    public RectTransform backgroundBox;

    private Message[] currentMessages;
    private Actor[] currentActors;

    private int activeMessages;
    public static bool isActive;
    
    private void Update()
    {
        if (PlayerManager.Instance.player.OnPlayerInputs.Player.NextMessage.WasPressedThisFrame() && isActive)
        {
            NextMessage();
        }
    }

    public void OpenDialogue(Message[] messages, Actor[] actors)
    {
        currentMessages = messages;
        currentActors = actors;
        activeMessages = 0;
        isActive = true;

        NextMessage();
        
        Debug.Log("Started Conversation! LoadedMessages: " + messages.Length);
    }

    public void DisplayMessages()
    {
        Message messageToDisplay = currentMessages[activeMessages];
        messageText.text = messageToDisplay.actorMessage;

        Actor actorToDisplay = currentActors[messageToDisplay.actorId];
        actorName.text = actorToDisplay.name;
        actorImage.sprite = actorToDisplay.actorSprite;
    }

    public void NextMessage()
    {
        if (activeMessages < currentMessages.Length)
        {
            DisplayMessages();
        }
        else
        {
            isActive = false;
            Debug.Log("Conversation Ended");
        }
        
        activeMessages++;
    }
}
