using UnityEngine;
using UnityEngine.UI;

public class SampleCamera : MonoBehaviour
{
    public Button openCamera, openPhoto;
    public Button savePhoto;
    public RawImage rawImage;
    public RawImage croppedImageHolder;
    public Text croppedImageSizeText;
    public Toggle cutTypeToggle;
    void Start()
    {
        openCamera.onClick.AddListener(OpenCamera);
        openPhoto.onClick.AddListener(OpenPhoto);
        savePhoto.onClick.AddListener(SavePhoto);
    }
    //打开相机
    private void OpenCamera()
    {
        NativeCall.OpenCamera((Texture2D tex) =>
        {
            rawImage.texture = tex;
            rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
            Instantiate(Resources.Load<GameObject>("ImageCropper"));
            CutPhoto(tex);
        });
    }
    //打开相册
    private void OpenPhoto()
    {
        NativeCall.OpenPhoto((Texture2D tex) =>
        {
            Debug.LogError("图片读取完成");
            CutPhoto(tex);
            try
            {
                byte[] bytes222 = tex.EncodeToPNG();
                Debug.LogError("bytes222.lenght:" + bytes222.Length);
            }
            catch (System.Exception e)
            {
                Debug.LogError(e.ToString());
            }

            Debug.LogError("转换完成.lenght:");
            rawImage.texture = tex;
            //rawImage.rectTransform.sizeDelta = new Vector2(tex.width, tex.height);
        });
    }

    //保存照片
    private void SavePhoto()
    {
        NativeCall.SavePhoto(rawImage.texture as Texture2D);
    }

    public void CutPhoto(Texture texture)
    {
        ImageCropper.Instance.Show(texture, (bool result, Texture originalImage, Texture2D croppedImage) =>
        {

            // Destroy previously cropped texture (if any) to free memory
            Destroy(croppedImageHolder.texture, 5f);

            // If screenshot was cropped successfully
            if (result)
            {
                // Assign cropped texture to the RawImage
                croppedImageHolder.enabled = true;
                croppedImageHolder.texture = croppedImage;

                Vector2 size = croppedImageHolder.rectTransform.sizeDelta;
                if (croppedImage.height <= croppedImage.width)
                    size = new Vector2(400f, 400f * (croppedImage.height / (float)croppedImage.width));
                else
                    size = new Vector2(400f * (croppedImage.width / (float)croppedImage.height), 400f);
                croppedImageHolder.rectTransform.sizeDelta = size;

                croppedImageSizeText.enabled = true;
                croppedImageSizeText.text = "Image size: " + croppedImage.width + ", " + croppedImage.height;
            }
            else
            {
                croppedImageHolder.enabled = false;
                croppedImageSizeText.enabled = false;
            }

            // Destroy the screenshot as we no longer need it in this case
            Destroy(texture);
        },
                settings: new ImageCropper.Settings()
                {
                    ovalSelection = cutTypeToggle.isOn,
                    autoZoomEnabled = false,
                    imageBackground = Color.clear, // transparent background
                    selectionMinAspectRatio = 1,
                    selectionMaxAspectRatio = 1

                },
                croppedImageResizePolicy: (ref int width, ref int height) =>
                {
                    // uncomment lines below to save cropped image at half resolution
                    //width /= 2;
                    //height /= 2;
                });
    }
}
