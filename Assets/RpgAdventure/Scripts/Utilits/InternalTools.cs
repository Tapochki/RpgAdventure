using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TandC.Project.Utilities
{
    public static class InternalTools
    {
        private static string LINE_BREAK = "%n%";

        //public static Sequence DoActionDelayed(TweenCallback action, float delay = 0f)
        //{
        //    if (action == null)
        //    {
        //        return null;
        //    }

        //    Sequence sequence = DOTween.Sequence();
        //    sequence.PrependInterval(delay);
        //    sequence.AppendCallback(action);

        //    return sequence;
        //}

        public static void HapticVibration(int level = 0)
        {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            Handheld.Vibrate();
#endif
        }

        public static float ConvertToRadians(float angle)
        {
            return (Mathf.PI / 180) * angle;
        }

        public static float GetFloatSquaredNumber(float num, int n)
        {
            float num_n = 1;
            for (int i = 0; i < n; i++)
            {
                num_n *= num;
            }
            return num_n;
        }

        public static Vector2 GPSToUnityCoordinatesWithArangeInRectangleShape(Vector2 gpsP1, Vector2 gpsP2, Vector2 uiP1, Vector2 uiP4, Vector2 mainGPS)
        {
            Vector2 calculatedPosition = Vector2.zero;
            calculatedPosition.x = uiP1.x + ((uiP4.x - uiP1.x) * ((mainGPS.x - gpsP1.x) / (gpsP2.x - gpsP1.x)));
            calculatedPosition.y = uiP1.y + ((uiP4.y - uiP1.y) * ((mainGPS.y - gpsP1.y) / (gpsP2.y - gpsP1.y)));

            calculatedPosition.x = Mathf.Clamp(calculatedPosition.x, Mathf.Min(uiP1.x, uiP4.x), Mathf.Max(uiP1.x, uiP4.x));
            calculatedPosition.y = Mathf.Clamp(calculatedPosition.y, Mathf.Min(uiP1.y, uiP4.y), Mathf.Max(uiP1.y, uiP4.y));

            return calculatedPosition;
        }

        public static int GetIntSquaredNumber(int num, int n)
        {
            int num_n = 1;
            for (int i = 0; i < n; i++)
            {
                num_n *= num;
            }
            return num_n;
        }

        /// <summary>
        /// Multiplier must be 1.07 - 1.15
        /// </summary>
        public static float GetIncrementalFloatValue(float basicValue, float multiplier, int ownedCount)
        {
            return basicValue * GetFloatSquaredNumber(multiplier, ownedCount);
        }

        public static T EnumFromString<T>(string value) where T : Enum
        {
            return (T)Enum.Parse(typeof(T), value);
        }

        public static void MoveToEndOfList<T>(IList<T> list, int index)
        {
            T item = list[index];
            list.RemoveAt(index);
            list.Add(item);
        }

        public static string ReplaceLineBreaks(string data)
        {
            if (data == null)
            {
                return "";
            }

            return data.Replace(LINE_BREAK, "\n");
        }

        public static void ShuffleList<T>(this IList<T> list)
        {
            System.Random rnd = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static List<T> GetRandomElements<T>(this IList<T> list, int count)
        {
            List<T> shuffledList = new List<T>(count);
            shuffledList.AddRange(list);

            if (list.Count <= count)
            {
                return shuffledList;
            }

            ShuffleList(shuffledList);
            return shuffledList.GetRange(0, count);
        }

        public static float LerpByStep(float current, float end, float step, bool applyDeltaTime = true)
        {
            if (applyDeltaTime)
            {
                step *= Time.deltaTime;
            }

            if (current > end)
            {
                step *= -1f;
            }

            return Mathf.Clamp(current + step, Mathf.Min(current, end), Mathf.Max(current, end));
        }

        public static T GetInstance<T>(string name, params object[] args)
        {
            return (T)Activator.CreateInstance(Type.GetType(name), args);
        }

        public static float GetAngleBetweenTwoVectors2(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            Vector2 playerToArrowDirection = a1 - a2;
            Vector2 playerToPOIDirection = b1 - b2;

            float angleBetweenPlayerAndArrow = Mathf.Atan2(playerToArrowDirection.y, playerToArrowDirection.x) * Mathf.Rad2Deg - 90;
            float angleBetweenPlayerAndPOI = Mathf.Atan2(playerToPOIDirection.y, playerToPOIDirection.x) * Mathf.Rad2Deg - 90;

            float result = angleBetweenPlayerAndArrow - angleBetweenPlayerAndPOI;

            if (result > 180f)
            {
                result = (360 - result);
            }

            if (result < -180)
            {
                result = (360 + result);
            }

            return result;
        }

        //public static Tweener DOTextInt(this Text text, int initialValue, int finalValue, float duration)
        //{
        //    return DOTween.To(
        //         () => initialValue,
        //         it =>
        //         {
        //             if (text != null && text)
        //             {
        //                 text.text = it.ToString();
        //             }
        //         },
        //         finalValue,
        //         duration
        //     );
        //}

        //public static Tweener DOTextInt(this TextMeshProUGUI text, int initialValue, int finalValue, float duration)
        //{
        //    return DOTween.To(
        //         () => initialValue,
        //         it =>
        //         {
        //             if (text != null && text)
        //             {
        //                 text.text = it.ToString();
        //             }
        //         },
        //         finalValue,
        //         duration
        //     );
        //}

        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        public static Rect GetScreenCoordinates(RectTransform uiElement, GameObject canvas)
        {
            RectTransform canvasTransf = canvas.GetComponent<RectTransform>();

            Vector2 canvasSize = new Vector2(canvasTransf.rect.width, canvasTransf.rect.height);
            float koefX = Screen.width / canvasSize.x;
            float koefY = Screen.height / canvasSize.y;
            Vector2 position = Vector2.Scale(uiElement.anchorMin, canvasSize);
            float directionX = uiElement.pivot.x * -1;
            float directionY = uiElement.pivot.y * -1;

            var result = new Rect();
            result.width = uiElement.sizeDelta.x * koefX;
            result.height = uiElement.sizeDelta.y * koefX;
            result.x = position.x * koefX + uiElement.anchoredPosition.x * koefX + result.width * directionX;
            result.y = position.y * koefY + uiElement.anchoredPosition.y * koefX + result.height * directionY;
            return result;
        }

        public static float DeviceDiagonalSizeInInches()
        {
            float screenWidth = Screen.width / Screen.dpi;
            float screenHeight = Screen.height / Screen.dpi;
            float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

            return diagonalInches;
        }

        //public static string FormatStringToPascaleCase(string root)
        //{
        //    return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(root.ToLower().Replace("_", " ")).Replace(" ", string.Empty);
        //}

        public static bool IsTabletScreen()
        {
#if FORCE_TABLET_UI
            return true;
#elif FORCE_PHONE_UI
            return false;
#else
            return DeviceDiagonalSizeInInches() > 6.5f;
#endif
        }

        /// <summary>
        /// Based on Top left and Down right UI and GPS point place object in rectangle shape via GPS coordinates
        /// <br>uiP1 - top left UI point </br>
        /// <br>uiP4 - down right UI point </br>
        /// <br>gpsP1 - top left GPS point </br>
        /// <br>gpsP4 - down right GPS point </br>
        /// <br>playerNativeGPSPosition - position of player based on GPS system </br>
        /// </summary>
        /// <param name="uiP1">Top left UI point</param>
        /// <param name="uiP4">Down right UI point</param>
        /// <param name="gpsP1">Top left GPS point</param>
        /// <param name="gpsP4">Down right GPS point</param>
        /// <param name="playerNativeGPSPosition"></param>
        /// <returns></returns>
        public static Vector2 GetPositionInUIRectangleShapeBasedOnGPSPosition(Vector2 uiP1, Vector2 uiP4, Vector2 gpsP1, Vector2 gpsP4,
            Vector2 playerNativeGPSPosition)
        {
            Vector2 result;

            result.x = uiP1.x + ((uiP4.x - uiP1.x) * ((playerNativeGPSPosition.x - gpsP1.x) / (gpsP4.x - gpsP1.x)));
            result.y = uiP1.y + ((uiP4.y - uiP1.y) * ((playerNativeGPSPosition.y - gpsP1.y) / (gpsP4.y - gpsP1.y)));

            result.x *= -1f;
            result.y *= -1f;

            result.x = Mathf.Clamp(result.x, Mathf.Min(uiP1.x, uiP4.x), Mathf.Max(uiP1.x, uiP4.x));
            result.y = Mathf.Clamp(result.y, Mathf.Min(uiP1.y, uiP4.y), Mathf.Max(uiP1.y, uiP4.y));

            return result;
        }

        public static string GetUserCode(int coseSize)
        {
            string code = string.Empty;

            const string numbers = "0123456789";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            for (int i = 0; i < coseSize; i++)
            {
                if (UnityEngine.Random.Range(0, 100) < 50)
                {
                    code += numbers[UnityEngine.Random.Range(0, numbers.Length)];
                }
                else
                {
                    code += chars[UnityEngine.Random.Range(0, chars.Length)];
                }
            }

            return code;
        }

        public static string GetUserId()
        {
            return SystemInfo.deviceUniqueIdentifier;
        }

        public static string SerializeData(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        public static T DeserializeData<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        public static string FormatNum(float num)
        {
            if (num == 0)
            {
                return "0";
            }

            int i = 0;

            while (i + 1 < NUMBER_ABBREVIATIONS.Length && num >= 1000f)
            {
                num /= 1000f;
                i++;
            }

            return num.ToString("#.##") + NUMBER_ABBREVIATIONS[i];
        }

        private static string[] NUMBER_ABBREVIATIONS = { "", "K", "M", "B", "T", "Qua", "Quin", "Sex", "Sept", "Oct", "Non", "Deci" };

        private static Camera _camera;

        public static Camera Camera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = Camera.main;
                }

                return _camera;
            }
        }

        private static PointerEventData _currentPositionEventData;
        private static List<RaycastResult> _results;

        public static bool IsOverUI()
        {
            _currentPositionEventData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };
            _results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(_currentPositionEventData, _results);
            return _results.Count > 0;
        }

        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
            return result;
        }

        public static void DeleteAllChilds(this Transform parent)
        {
            foreach (Transform child in parent)
            {
                MonoBehaviour.Destroy(child.gameObject);
            }
        }

        #region UnixTime

        public static long ConvertToUnixFormat(DateTime? dateTime)
        {
            if (dateTime == null)
            {
                return 0;
            }

            return (long)(dateTime.Value - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        public static DateTime ConvertFromUnixFormat(long? unixTime)
        {
            if (unixTime == null)
            {
                return DateTime.Now;
            }

            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return start.AddSeconds(unixTime.Value);
        }

        #endregion UnixTime

        public static void SetLayerRecursively(this GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursively(child.gameObject, layer);
            }
        }

        public static string LimitStringLength(string str, int maxLength)
        {
            if (str.Length < maxLength)
            {
                return str;
            }

            return str.Substring(0, maxLength);
        }

        public static double GetTimestamp()
        {
            return new TimeSpan(DateTime.UtcNow.Ticks).TotalSeconds;
        }

        #region cryptography

        public static string Encrypt(string value, string key)
        {
            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(value), key));
        }

        [DebuggerNonUserCode]
        public static string Decrypt(string value, string key)
        {
            string result;

            try
            {
                using (CryptoStream cryptoStream = InternalDecrypt(Convert.FromBase64String(value), key))
                {
                    using (StreamReader streamReader = new StreamReader(cryptoStream))
                    {
                        result = streamReader.ReadToEnd();
                    }
                }
            }
            catch (CryptographicException e)
            {
                UnityEngine.Debug.LogException(e);
                return null;
            }

            return result;
        }

        private static byte[] Encrypt(byte[] key, string value)
        {
            SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create();
            ICryptoTransform cryptoTransform =
                symmetricAlgorithm.CreateEncryptor(new Rfc2898DeriveBytes(value, new byte[16]).GetBytes(16), new byte[16]);

            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream =
                    new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(key, 0, key.Length);
                    cryptoStream.FlushFinalBlock();

                    result = memoryStream.ToArray();

                    memoryStream.Close();
                    memoryStream.Dispose();
                }
            }

            return result;
        }

        private static CryptoStream InternalDecrypt(byte[] key, string value)
        {
            SymmetricAlgorithm symmetricAlgorithm = Rijndael.Create();
            ICryptoTransform cryptoTransform =
                symmetricAlgorithm.CreateDecryptor(new Rfc2898DeriveBytes(value, new byte[16]).GetBytes(16),
                    new byte[16]);

            MemoryStream memoryStream = new MemoryStream(key);
            return new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read);
        }

        public static byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0:
                    break;

                case 2:
                    output += "==";
                    break;

                case 3:
                    output += "=";
                    break;

                default:
                    throw new Exception("Illegal base64url string!");
            }
            byte[] converted = Convert.FromBase64String(output);
            return converted;
        }

        #endregion cryptography
    }

    internal static class CanvasExtensions
    {
        public static Vector2 SizeToParent(this RawImage image, float padding = 0)
        {
            var parent = image.transform.parent.GetComponent<RectTransform>();
            var imageTransform = image.GetComponent<RectTransform>();
            if (!parent) { return imageTransform.sizeDelta; } //if we don't have a parent, just return our current width;
            padding = 1 - padding;
            float w = 0, h = 0;
            float ratio = image.texture.width / (float)image.texture.height;
            var bounds = new Rect(0, 0, parent.rect.width, parent.rect.height);
            if (Mathf.RoundToInt(imageTransform.eulerAngles.z) % 180 == 90)
            {
                //Invert the bounds if the image is rotated
                bounds.size = new Vector2(bounds.height, bounds.width);
            }
            //Size by height first
            h = bounds.height * padding;
            w = h * ratio;
            if (w > bounds.width * padding)
            { //If it doesn't fit, fallback to width;
                w = bounds.width * padding;
                h = w / ratio;
            }
            imageTransform.sizeDelta = new Vector2(w, h);
            return imageTransform.sizeDelta;
        }
    }
}