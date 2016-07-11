using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.SqlServer.Server;

namespace Telectronica.Peaje
{

    public class ImagenBS
    {

        //private Bitmap m_Image;

        public ImagenBS(Bitmap im)
        {
           // m_Image = im;
        }


        public static Bitmap setAjustes(string uri, int brillo, double contraste, bool byn,bool inv)
        {
            Bitmap imgAjustada;
            try
            {
                imgAjustada  = new Bitmap(uri);
                if (brillo != 0)
                {
                    imgAjustada = ajusteBrillo(imgAjustada, brillo);
                }
                if (Convert.ToInt32(contraste) != 0)
                {
                    imgAjustada = ajusteContraste(imgAjustada, contraste);
                }

                if (byn)
                {
                    imgAjustada = ajustarBlancoyNegro(imgAjustada);
                }

                if (inv)
                {
                    imgAjustada = invertirColores(imgAjustada);
                }
            }
            catch (Exception )
            {
                
                throw;
            }
            
            
            return imgAjustada;
        }







        public static Bitmap setContraste(string uri, double contraste)
        {
            Bitmap imgAjustada = new Bitmap(uri);
            return ajusteContraste(imgAjustada, contraste);
        }

        public static Bitmap setBrillo(string uri, int brillo)
        {
            Bitmap imgAjustada = new Bitmap(uri);
            //imgAjustada = cargarImagen(uri);
            return ajusteBrillo(imgAjustada, brillo);

        }




        //Cambia la foto a blanco y negro
        public static Bitmap ajustarBlancoyNegro(Bitmap m_Image)
        {

            Bitmap map = (Bitmap)m_Image.Clone();
            Color c;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    c = map.GetPixel(i, j);
                    byte gray = (byte) (.299*c.R + .587*c.G + .114*c.B);

                    map.SetPixel(i,j,Color.FromArgb(gray,gray,gray));
                }
            }

            return (Bitmap) map.Clone();
        }

        /// <summary>
        /// Invierte los colores de la imagen origen
        /// </summary>
        /// <param name="m_Image"></param>
        /// <returns></returns>
        public static Bitmap invertirColores(Bitmap m_Image)
        {
            Bitmap map = (Bitmap) m_Image.Clone();
            Color c;

            for (int i = 0; i < map.Width; i++) 
            {
                for (int j = 0; j < map.Height; j++)
                {
                    c = map.GetPixel(i, j);
                    map.SetPixel(i,j,Color.FromArgb(255-c.R,255-c.G,255-c.B));
                }
            }

            return (Bitmap) map.Clone();
        }

        //Ajusta el brillo de un bitmap
        public static Bitmap ajusteBrillo(Bitmap m_Image,int brillo)
        {
            Bitmap temp =  m_Image;
            Bitmap bmap = (Bitmap) temp.Clone();

            if (brillo < -255) brillo = -255;
            if (brillo > 255) brillo = 255;
            Color color;

            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    color = bmap.GetPixel(i, j);
                    int cR = color.R + brillo;
                    int cG = color.G + brillo;
                    int cB = color.B + brillo;


                    if (cR < 0) cR = 1;
                    if (cR > 255) cR = 255;
                    if (cG < 0) cG = 1;
                    if (cG > 255) cG = 255;
                    if (cB < 0) cB = 1;
                    if (cB > 255) cB = 255;

                    bmap.SetPixel(i, j, Color.FromArgb((byte)cR, (byte)cG, (byte)cB));

                }
            }

            return (Bitmap)bmap.Clone();
        }



        public static Bitmap ajusteContraste( Bitmap _currentBitmap ,double contrast)
        {
            Bitmap temp = _currentBitmap;
            Bitmap bmap = (Bitmap)temp.Clone();
            if (contrast < -100) contrast = -100;
            if (contrast > 100) contrast = 100;
            contrast = (100.0 + contrast) / 100.0;
            contrast *= contrast;
            Color c;
            for (int i = 0; i < bmap.Width; i++)
            {
                for (int j = 0; j < bmap.Height; j++)
                {
                    c = bmap.GetPixel(i, j);
                    double pR = c.R / 255.0;
                    pR -= 0.5;
                    pR *= contrast;
                    pR += 0.5;
                    pR *= 255;
                    if (pR < 0) pR = 0;
                    if (pR > 255) pR = 255;

                    double pG = c.G / 255.0;
                    pG -= 0.5;
                    pG *= contrast;
                    pG += 0.5;
                    pG *= 255;
                    if (pG < 0) pG = 0;
                    if (pG > 255) pG = 255;

                    double pB = c.B / 255.0;
                    pB -= 0.5;
                    pB *= contrast;
                    pB += 0.5;
                    pB *= 255;
                    if (pB < 0) pB = 0;
                    if (pB > 255) pB = 255;

                    bmap.SetPixel(i, j,
                    Color.FromArgb((byte)pR, (byte)pG, (byte)pB));
                }
            }
            return bmap;

        }
        
    }

    


 
}
