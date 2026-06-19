using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
  [SerializeField] private Image[] _images;
  [SerializeField] private float _parallaxSpeed = 5f;

  private DoubleLinkedList<Image> _imageListDic;
  private float _movedDistance;

  private void Start()
  {
    _movedDistance = 0f;
    _imageListDic = new();

    for (int i = 0; i < _images.Length; ++i)
    {
      AddLast(_images[i]);
    }
  }

  private void Update()
  {
    if (_images == null || _images.Length == 0)
    {
      return;
    }

    float distance = _parallaxSpeed * Time.deltaTime;
    _movedDistance += distance;

    for (int i = 0; i < _images.Length; ++i)
    {
      Vector2 anchoredPosition = _images[i].GetComponent<RectTransform>().anchoredPosition;
      _images[i].GetComponent<RectTransform>().anchoredPosition = anchoredPosition - new Vector2(distance, 0f);
    }

    float width = _images[0].GetComponent<RectTransform>().rect.width;
    if (_movedDistance >= width)
    {
      _movedDistance = 0f;
      ShiftLeft();
    }
  }
  private void ShiftLeft()
  {
    float width = _images[0].GetComponent<RectTransform>().rect.width;
    Image tailImage = _imageListDic.Tail.Data;
    Image headImage = _imageListDic.Head.Data;
    headImage.GetComponent<RectTransform>().anchoredPosition = tailImage.GetComponent<RectTransform>().anchoredPosition + new Vector2(width, 0f);

    _imageListDic.ShiftLeft();
  }

  private void AddLast(Image image)
  {
    float width = _images[0].GetComponent<RectTransform>().rect.width;
    RectTransform imageRectTransform = image.GetComponent<RectTransform>();

    if (_imageListDic.Tail is null)
    {
      imageRectTransform.anchoredPosition = Vector2.zero;
    }
    else
    {
      Image tailImage = _imageListDic.Tail.Data;
      imageRectTransform.anchoredPosition = tailImage.GetComponent<RectTransform>().anchoredPosition + new Vector2(width, 0f);
    }

    _imageListDic.AddLast(image);
  }
}