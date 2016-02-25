using System.Drawing;

namespace Netron.GraphLib.UI
{
    public static class PageSize
    {
        /*
        1mm = dpi / 25.4 (pixel)

        A4 = 210 * 297 mm
        A3 = 297 * 420 mm

        */
        private static int m_dpix = 0;
        private static int m_dpiy = 0;
        private static Size m_A4 = new Size();
        private static Size m_A3 = new Size();
        private static Size m_MSWordPage = new Size();

        private static void Init(int dpix, int dpiy)
        {
            if (m_dpix != dpix)
            {
                m_dpix = dpix;
                m_A4.Width = (int)(m_dpix * (210 / 25.4));
                m_A3.Width = (int)(m_dpix * (297 / 25.4));
                m_MSWordPage.Width = (int) (m_A4.Width * (555.0 / 792.0));
            }

            if (m_dpiy != dpiy)
            {
                m_dpiy = dpiy;
                m_A4.Height = (int)(m_dpiy * (297 / 25.4));
                m_A3.Height = (int)(m_dpiy * (420 / 25.4));
                m_MSWordPage.Height = (int)(m_A4.Height * (465.0 / 550.0));

            }
        }

        /// <summary>
        /// Get the pixel number for A4 size according to dpi
        /// </summary>
        /// <param name="dpix"></param>
        /// <param name="dpiy"></param>
        /// <returns></returns>
        public static Size GetA4(int dpix, int dpiy)
        {
            Init(dpix, dpiy);
            return m_A4;
        }

        /// <summary>
        /// Get the pixel number for A3 size according to dpi
        /// </summary>
        /// <param name="dpix"></param>
        /// <param name="dpiy"></param>
        /// <returns></returns>
        public static Size GetA3(int dpix, int dpiy)
        {
            Init(dpix, dpiy);
            return m_A3;
        }

        /// <summary>
        /// Get the pixel number for Microsoft Word page size according to dpi
        /// </summary>
        /// <param name="dpix"></param>
        /// <param name="dpiy"></param>
        /// <returns></returns>
        public static Size GetMSWordPage(int dpix, int dpiy)
        {
            Init(dpix, dpiy);
            return m_MSWordPage;
        }

    }
}