using UnityEngine;

namespace OutGame
{
    using Cell = TEMPLATE_SCRIPT_CELL;
    using CellData = TEMPLATE_SCRIPT_CELL_DATA;
    using CellFactory = TEMPLATE_SCRIPT_CELL_FACTORY;

    public class TEMPLATE_SCRIPT_SCROLLER_MODEL : ScrollerModelBase<Cell, CellData, CellFactory>
    {
        public override float ScrollSensitivity { get; }
        protected override int startVerticalCellCount => 8;
        protected override int horizontalCellCount => 1;
        protected override float horizontalCellSpace => 0;
        protected override float verticalCellSpace => 0;

        public TEMPLATE_SCRIPT_SCROLLER_MODEL(Cell cell, Transform parent) : base(cell, parent)
        {
            //var dataObject = DataObjectManager.Instance.OutGameDataObject;
            //ScrollSensitivity = dataObject.ScrollSensitivity;
        }
    }
}