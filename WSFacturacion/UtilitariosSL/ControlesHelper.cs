using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Linq;

namespace Telectronica.Utilitarios.SL
{
    /// <summary>
    ///Clases de controles visuales
    /// </summary>
    public static class ControlesHelper
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el control especificado de un item de un ItemsControl, ListBox, HeaderedItemsControl, TreeViwew, StatusBar, ComboBox, TabControl
        /// </summary>
        /// <param name="parent">ItemsControl - Control donde se busca</param>
        /// <param name="index">int - Posicion del Item</param>
        /// <param name="name">String - Nombre del control buscado</param>
        /// <returns>Control con ese nombre</returns>
        /// ***********************************************************************************************
        public static DependencyObject FindNameAllLevels(ItemsControl parent, int index, string name)
        {
            ContentPresenter cp = (ContentPresenter)parent.ItemContainerGenerator.ContainerFromIndex(index);
            //Dentro de cada item hay un Grid, un canvas y luego los controles
            //cp.ContentTemplate.ApplyTemplate();
            return FindNameAllLevels(cp, name);
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el control especificado de un item de un ItemsControl, ListBox, HeaderedItemsControl, TreeViwew, StatusBar, ComboBox, TabControl
        /// </summary>
        /// <param name="parent">ItemsControl - Control donde se busca</param>
        /// <param name="item">object - Item donde se busca </param>
        /// <param name="name">String - Nombre del control buscado</param>
        /// <returns>Control con ese nombre</returns>
        /// ***********************************************************************************************
        public static DependencyObject FindNameAllLevels(ItemsControl parent, object item, string name)
        {
            ContentPresenter cp = (ContentPresenter)parent.ItemContainerGenerator.ContainerFromItem(item);
            //Dentro de cada item hay un Grid, un canvas y luego los controles
            return FindNameAllLevels(cp, name);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el control especificado hijo del control parent cualquiera sea el nivel de anidacion
        /// </summary>
        /// <param name="parent">DependencyObject - Control donde se busca</param>
        /// <param name="name">String - Nombre del control buscado</param>
        /// <returns>Control con ese nombre</returns>
        /// ***********************************************************************************************
        public static DependencyObject FindNameAllLevels(DependencyObject parent, string name)
        {
            DependencyObject ret = null;
            if (parent != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
                {
                    DependencyObject dp = VisualTreeHelper.GetChild(parent, i);
                    if (dp is FrameworkElement)
                    {
                        FrameworkElement fe = (FrameworkElement)dp;
                        if (fe.Name == name)
                        {
                            ret = dp;
                            break;
                        }
                    }

                    //Buscamos los hijos
                    ret = FindNameAllLevels(dp, name);
                    if (ret != null)
                        break;
                }
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el control especificado hijo del padre de control cualquiera sea el nivel de anidacion
        /// </summary>
        /// <param name="control">DependencyObject - Control a partir del cual se busca</param>
        /// <param name="name">String - Nombre del control buscado</param>
        /// <returns>Control con ese nombre</returns>
        /// ***********************************************************************************************
        public static DependencyObject FindSibling(DependencyObject control, string name)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(control);
            return FindNameAllLevels(parent, name);
        }

        public static DependencyObject getPrimerChild(DependencyObject parent)
        {
            if (parent != null)
                if (VisualTreeHelper.GetChildrenCount(parent) > 0)
                    return VisualTreeHelper.GetChild(parent, 0);

            return null;
        }

        public static FrameworkElement getCellContent(DataGrid dg, int columna, DataGridRow row)
        {
            DataGridColumn column = dg.Columns[columna];
            FrameworkElement fe = column.GetCellContent(row);
            return fe;
        }

        public static FrameworkElement getCellContent(DataGrid dg, int columna, object dataItem)
        {
            DataGridColumn column = dg.Columns[columna];
            FrameworkElement fe = column.GetCellContent(dataItem);
            return fe;
        }

        public static DataGridCell getCell(DataGrid dg, int columna, DataGridRow row)
        {
            DataGridColumn column = dg.Columns[columna];
            DataGridCell cell = (DataGridCell) GetParent( column.GetCellContent(row), typeof(DataGridCell));
            return cell;
        }

        public static DataGridCell getCell(DataGrid dg, int columna, object dataItem)
        {
            DataGridColumn column = dg.Columns[columna];
            DataGridCell cell = (DataGridCell)GetParent(column.GetCellContent(dataItem), typeof(DataGridCell));
            return cell;
        }

        public static void GetGridRowColumnIndex(Point pt, DataGrid grid, out int rowIndex, out int colIndex)
        {
            object dataContext;
            GetGridRowColumnIndex(pt, grid, out rowIndex, out colIndex, out dataContext);
        }

        public static FrameworkElement GetParent(FrameworkElement child, Type type)
        {
            var parent = child.Parent;
            if (null != parent)
            {
                if (parent.GetType() == type)
                {
                    return parent as FrameworkElement;
                }
                else
                {
                    return GetParent(parent as FrameworkElement, type);
                }
            }
            return null;
        }

        public static FrameworkElement GetChild(FrameworkElement parent, Type type)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {

                var child = VisualTreeHelper.GetChild(parent, i);

                if (null != child)
                {
                    if (child.GetType() == type)
                    {
                        return child as FrameworkElement;
                    }
                    else
                    {
                        FrameworkElement child2 = GetChild(child as FrameworkElement, type);
                        if (child2 != null)
                            return child2;
                    }
                }
            }
            return null;
        }

        //Devuelve el objeto bindeado a la fila de la grilla solo si hicimos click en la columna deseada
        //Usar en los eventos del mouse de las columbnas de los checkbox
        public static void GetGridRowColumnIndex(MouseButtonEventArgs e, DataGrid grid, out int rowIndex, out int colIndex)
        {
            GetGridRowColumnIndex(e.GetPosition(null), grid, out rowIndex, out colIndex);
            if (colIndex < 0)
            {
                //A Veces falla
                //PLAN B
                //Probamos por la posicion
                double ancho = 0.0, pos;
                pos = e.GetPosition(grid).X;
                for (int i = 0; i < grid.Columns.Count; i++)
                {
                    ancho += grid.Columns[i].ActualWidth;
                    if (pos < ancho)
                    {
                        colIndex = i;
                        break;
                    }
                    ancho += grid.BorderThickness.Left;
                }
                if (rowIndex < 0)
                    rowIndex = grid.SelectedIndex;
            }
        }
        private static void GetGridRowColumnIndex(Point pt, DataGrid grid, out int rowIndex, out int colIndex, out object dataContext)
        {
            rowIndex = -1;
            colIndex = -1;
            dataContext = null;

            //Convertir coordenadas
            //GeneralTransform generalTransform = grid.TransformToVisual(Application.Current.RootVisual);

            //Point childToParentCoordinates = generalTransform.Transform(pt);

            //Matrix matrix = ((MatrixTransform)grid.TransformToVisual(Application.Current.RootVisual)).Matrix;
            //Point childToParentCoordinates = new Point(matrix.OffsetX+pt.X,matrix.OffsetY+pt.Y);
            var elements = VisualTreeHelper.FindElementsInHostCoordinates(pt, grid);
            if (null == elements ||
               elements.Count() == 0)
            {
                return;
            }


            // Get the rows and columns.
            var rowQuery = from gridRow in elements where gridRow is DataGridRow select gridRow as DataGridRow;
            var cellQuery = from gridCell in elements where gridCell is DataGridCell select gridCell as DataGridCell;
            var cells = cellQuery.ToList<DataGridCell>();





            foreach (var row in rowQuery)
            {
                dataContext = row.DataContext;
                rowIndex = row.GetIndex();
                if (cells.Count == 0)
                {
                    return;
                }
                foreach (DataGridColumn col in grid.Columns)
                {
                    var colContent = col.GetCellContent(row);
                    var parent = GetParent(colContent, typeof(DataGridCell));
                    if (parent != null)
                    {
                        var thisCell = (DataGridCell)parent;
                        if (object.ReferenceEquals(thisCell, cells[0]))
                        {
                            colIndex = col.DisplayIndex;
                        }
                    }
                }
            }
        }


    }

}
