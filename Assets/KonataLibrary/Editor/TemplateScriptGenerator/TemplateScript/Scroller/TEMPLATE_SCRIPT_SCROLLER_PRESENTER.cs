using UnityEngine;

namespace OutGame
{
    using Cell = TEMPLATE_SCRIPT_CELL;
    using CellData = TEMPLATE_SCRIPT_CELL_DATA;
    using CellFactory = TEMPLATE_SCRIPT_CELL_FACTORY;
    using Model = ScrollerModelBase<TEMPLATE_SCRIPT_CELL, TEMPLATE_SCRIPT_CELL_DATA, TEMPLATE_SCRIPT_CELL_FACTORY>;
    using View = TEMPLATE_SCRIPT_SCROLLER_VIEW;
    
    public class TEMPLATE_SCRIPT_SCROLLER_PRESENTER : ScrollerPresenterBase<Cell, CellData, CellFactory>
    {
        [SerializeField] private Cell cellPrefab;
        [SerializeField] protected View view;

        protected override Model Model => new TEMPLATE_SCRIPT_SCROLLER_MODEL(cellPrefab, view.Content);
        protected override ScrollerViewBase View => view;
        
        protected override void OnInitialize()
        {
            view.RegisterScrollRect(OnUpdate);
        }

        protected override void OnEntry()
        {
        }

        protected override void OnUpdated()
        {
        }
    }
}