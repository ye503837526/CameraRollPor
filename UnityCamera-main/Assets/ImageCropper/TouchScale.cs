using UnityEngine;
using UnityEngine.EventSystems;

public class TouchScale : MonoBehaviour/*, IBeginDragHandler, IDragHandler, IEndDragHandler*/
{
    
    public float mDir = 0;
    private Vector3 mlastFramScale;
    private RectTransform mRect;
    private Vector3 mOffetScale;
    public Transform mPhotoTopPoint;
    public Transform mPhotoLeaftPoint;
    public Transform CropperUpPoint;
    public Transform CropperLeaftPoint;

    [Header("是否精准拖拽")]
    public bool m_isPrecision;

    //存储图片中心点与鼠标点击点的偏移量
    private Vector3 m_offset;

    //存储当前拖拽图片的RectTransform组件
    private RectTransform m_rt;

    void Start()
    {
        mlastFramScale = transform.localScale;
        mRect = GetComponent<RectTransform>();
        //初始化
        m_rt = gameObject.GetComponent<RectTransform>();
        Screen.orientation = ScreenOrientation.Portrait;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;

    }
    void FixedUpdate()
    {

        if (Input.touchCount >= 2)
        {
            Touch[] touches = Input.touches;
            Vector3 touchPos0 = touches[0].position;
            Vector3 touchPos1 = touches[1].position;
            touchPos0.z = 0;
            touchPos1.z = 0;
            if (mDir == 0)
                mDir = Vector2.Distance(touchPos0, touchPos1);
            float deltaDir = Vector2.Distance(touchPos0, touchPos1);
            float offest = (deltaDir - mDir) * 0.0015f;
            mOffetScale.x = mOffetScale.y = offest;
            Vector3 targetScale = mlastFramScale + mOffetScale;
            //Debug.Log("mPhotoTopPoint.y:" + mPhotoTopPoint.position.y + "   CropperUpPoint.y:" + CropperUpPoint.position.y);
            //Debug.Log("mPhotoLeaftPoint.x   :" + mPhotoLeaftPoint.position.x + "   CropperLeaftPoint.x:" + CropperLeaftPoint.position.x);
            //如果是缩放 就限制大宽或高
            if (offest < 0)
            {
                if (mPhotoTopPoint.position.y <= CropperUpPoint.position.y + 1)
                    return;
                if (mPhotoLeaftPoint.position.x <= CropperLeaftPoint.position.x + 1)
                    return;
            }
            transform.localScale = targetScale;
        }
        else
        {
            mlastFramScale = transform.localScale;
            mDir = 0;
        }
    }

    //private Vector3 StartDragPos;
    ////开始拖拽触发
    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    //如果精准拖拽则进行计算偏移量操作
    //    if (m_isPrecision)
    //    {
    //        //存储点击时的鼠标坐标
    //        Vector3 tWorldPos;
    //        //UI屏幕坐标转换为世界坐标
    //        RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rt, eventData.position, eventData.pressEventCamera, out tWorldPos);
    //        //计算偏移量
    //        m_offset = transform.position - tWorldPos;
    //    }
    //    //否则 默认偏移量为0
    //    else
    //    {
    //        m_offset = Vector3.zero;
    //    }
    //    StartDragPos = eventData.position;
    //    SetDraggedPosition(eventData);
    //}

    ////拖拽过程中触发
    //public void OnDrag(PointerEventData eventData)
    //{
    //    SetDraggedPosition(eventData);
    //}

    ////结束拖拽触发
    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    SetDraggedPosition(eventData);
    //}

    ///// <summary>
    ///// 设置图片位置方法
    ///// </summary>
    ///// <param name="eventData"></param>
    //private void SetDraggedPosition(PointerEventData eventData)
    //{

    //    //存储当前鼠标所在位置
    //    Vector3 globalMousePos;
    //    //UI屏幕坐标转换为世界坐标
    //    if (RectTransformUtility.ScreenPointToWorldPointInRectangle(m_rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
    //    {
    //        Vector3 endPos = globalMousePos + m_offset;
    //        if (Mathf.Abs(endPos.x - StartDragPos.x)>Mathf.Abs(endPos.y-StartDragPos.y))
    //        {
    //            //判断向左还是向右
    //            if (endPos.x-StartDragPos.x>0)
    //            {
    //                //右
    //                Debug.Log("右");
    //                if (mPhotoLeaftPoint.position.x <= CropperLeaftPoint.position.x)
    //                {
    //                    StartDragPos = endPos;
    //                    return;
    //                }
    //                if (mPhotoTopPoint.position.y <= CropperUpPoint.position.y)
    //                {
    //                    StartDragPos = endPos;
    //                    return;
    //                }
    //            }
    //            else
    //            {
    //                //左
    //                Debug.Log("左");
    //                if (mPhotoTopPoint.position.y <= CropperUpPoint.position.y)
    //                {
    //                    StartDragPos = endPos;
    //                    return;
    //                }

    //            }
    //        }
    //        else
    //        {
    //            if (endPos.y-StartDragPos.y>0)
    //            {
    //                Debug.Log("上");
                    
    //            }
    //            else
    //            {
    //                Debug.Log("下");
    //                if (mPhotoTopPoint.position.y <= CropperUpPoint.position.y)
    //                {
    //                    StartDragPos = endPos;
    //                    return;
    //                }
    //                if (mPhotoLeaftPoint.position.x <= CropperLeaftPoint.position.x)
    //                {
    //                    StartDragPos = endPos;
    //                    return;
    //                }
    //            }
    //        }


            
    //        //if (nextFramPos.y<StartDragPos.y)//向下拖动
    //        //{
    //        //    Debug.Log("检测上方");
    //        //    if (mPhotoTopPoint.position.y <= CropperUpPoint.position.y)
    //        //        return;
               
    //        //}
    //        //if (nextFramPos.x<StartDragPos.x)//向左拖动
    //        //{
    //        //    Debug.Log("检测左方");
    //        //    if (mPhotoLeaftPoint.position.x <= CropperLeaftPoint.position.x)
    //        //        return;
    //        //}
          
    //        //设置位置及偏移量
    //        m_rt.position = globalMousePos + m_offset;
    //    }
    //}
}
