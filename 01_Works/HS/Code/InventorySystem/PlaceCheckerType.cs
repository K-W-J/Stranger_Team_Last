namespace _01_Works.HS.Code.InventorySystem
{
    // 밖에 나갔는지, 곂친건지, 가능한건지 확인하는 거
    // 안보여줄지 아니면 재대로 보여줄지 아니면 빨간색으로 보여줄지 결정할려고 만듬
    public enum PlaceCheckerType 
    {
        CanPlace,
        CanNotPlace,
        OutOfBounds,
    }
}