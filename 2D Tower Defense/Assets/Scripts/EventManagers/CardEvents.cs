using System;

public static class CardEvents
{
    public static event Action<Card> OnCardHovered;
    public static event Action<Card> OnCardEnter;
    public static event Action<Card> OnCardClicked;
    public static event Action<Card> OnCardExited;
    public static void HoveredCard(Card card) => OnCardHovered?.Invoke(card);
    public static void EnterCard(Card card) => OnCardEnter?.Invoke(card);
    public static void ClickedCard(Card card) => OnCardClicked?.Invoke(card);
    public static void ExitedCard(Card card) => OnCardExited?.Invoke(card);
}
