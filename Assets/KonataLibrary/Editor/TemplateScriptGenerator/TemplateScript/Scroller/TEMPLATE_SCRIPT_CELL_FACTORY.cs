using UnityEngine;

namespace OutGame
{
    using Cell = TEMPLATE_SCRIPT_CELL;
    using CellData = TEMPLATE_SCRIPT_CELL_DATA;
    
    public class TEMPLATE_SCRIPT_CELL_FACTORY : ScrollerCellFactoryBase<Cell, CellData>
    {
        public TEMPLATE_SCRIPT_CELL_FACTORY(Cell prefab, Transform parent) : base(prefab, parent)
        {
        }

        public override void OnInitializeCell(Cell cell)
        {
        }
    }
}