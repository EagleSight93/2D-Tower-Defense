using System;

public static class CardEvents
{
    public static event Action<Card> OnCardClicked;
    public static event Action<Card> OnCardHovering;
    public static event Action<Card> OnCardEntered;
    public static event Action<Card> OnCardExited;
    public static void ClickedCard(Card card) => OnCardClicked?.Invoke(card);
    public static void HoveringCard(Card card) => OnCardHovering?.Invoke(card);
    public static void EnteredCard(Card card) => OnCardEntered?.Invoke(card);
    public static void ExitedCard(Card card) => OnCardExited?.Invoke(card);
}
