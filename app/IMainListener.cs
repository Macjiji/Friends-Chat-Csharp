namespace Friends_Chat
{
    public interface IMainListener
    {

        void OnDisconnect();
        void GoCreatingChat();
        void GoIntoChatRoom(ChatRoom chatRoom);

    }
}