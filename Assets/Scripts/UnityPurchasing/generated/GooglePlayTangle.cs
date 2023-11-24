// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("YnKvDTvOgLLCL0+V3hCENUfJjvkjI7C+cRx6BwTtk7YmNNY5U3RThBAWdKMSoiMYk0L7Xybaz6NXyMQ8y8c13UFgzt0PXqBlsBE6lqUEoikl05oDIvYosSzs+WoU0ksxkcR426opJygYqikiKqopKSiNRgCc/FCJGKopChglLiECrmCu3yUpKSktKCusiBQRzmq1PlyqzDhoQbmmMiXbExbYxLblV/u7VTgq6bXIpSdrB1ufL1TjYW215wiKz44wPD8uSWxvUwTyu7blRepuBi5jenaC60u2mYt6x8+HHi3Mda8px0xYEPd7KuBG8Dt+4lpRSQsd+7yy4SG8IpsEGyvoWj3EIR5CkvKe7ZopU/Y4bOnaHxIHKvoGBOGsrbE0jSorKSgp");
        private static int[] order = new int[] { 8,4,9,3,5,5,8,10,9,11,12,13,13,13,14 };
        private static int key = 40;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
