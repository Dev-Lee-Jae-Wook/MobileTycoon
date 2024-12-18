using EverythingStore.Save;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace EverythingStore.UI
{
	public class Navigation : MonoBehaviour
	{
		#region Field
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Transform _player;
		[SerializeField] private RectTransform _arrowIcon;
		[SerializeField] private RectTransform _dotIcon;
		[SerializeField] private Vector2 _dotRange;
		[SerializeField] private TMP_Text _distanceText;

		[Title("Target Icon")]
		[SerializeField] private RectTransform _rectTransfrom;

		private Camera _mainCam;
		private int _width;
		private int _height;
		private Image _image;
		private Transform _target;

		Vector3 disXZ = new Vector3(1.0f, 0.0f, 1.0f);
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_mainCam = Camera.main;
			_width = Screen.width;
			_height = Screen.height;
			_image = GetComponent<Image>();

			_arrowIcon.gameObject.SetActive(false);
			_dotIcon.gameObject.SetActive(false);
			OffNavigation();
		}

		// Update is called once per frame
		void Update()
		{
			NavigationUpdate();
		}

		private void NavigationUpdate()
		{
			Vector2 screenPos = WorldToScreenPos(_target.position);

			//영역 밖에 있는 경우 Dot 네비게이션 출력
			if (IsScreenBoundary(screenPos) == false)
			{
				if (screenPos.x > 0)
				{
					screenPos.x = Mathf.Min(screenPos.x, _width);
					screenPos.x -= _dotRange.x;
				}
				else
				{
					screenPos.x = Mathf.Max(0, screenPos.x);
					screenPos.x += _dotRange.x;
				}

				if (screenPos.y > 0)
				{
					screenPos.y = Mathf.Min(screenPos.y, _height);
					screenPos.y -= _dotRange.y;
				}
				else
				{
					screenPos.y = Mathf.Max(0, screenPos.y);
					screenPos.y += _dotRange.y;
				}

				DrawDotIcon();
			}
			//화살표 형태의 내비게이션 출력
			else
			{
				DrawArrowIcon();
			}

			_rectTransfrom.anchoredPosition = screenPos;
		}
		#endregion

		#region Public Method
		public void SetTarget(Transform target)
		{
			_target = target;

			NavigationUpdate();
			gameObject.SetActive(true);
			_canvas.enabled = true;
		}

		public void OffNavigation()
		{
			_canvas.enabled = false;
			gameObject.SetActive(false);
		}
		#endregion

		#region Private Method
		private Vector3 WorldToScreenPos(Vector3 worldPos)
		{
			return _mainCam.WorldToScreenPoint(worldPos);
		}

		private bool IsScreenBoundary(Vector2 screenPos)
		{

			//x over
			if (screenPos.x < 0f || screenPos.x > _width)
			{
				return false;
			}

			if (screenPos.y < 0f || screenPos.y > _height)
			{
				return false;
			}

			return true;
		}

		private void DrawDotIcon()
		{
			_arrowIcon.gameObject.SetActive(false);
			_dotIcon.gameObject.SetActive(true);
			UpdateTarget();
		}

		private void DrawArrowIcon()
		{
			_arrowIcon.gameObject.SetActive(true);
			_dotIcon.gameObject.SetActive(false);
		}

		private void UpdateTarget()
		{

			Vector3 start = Vector3.Cross(_player.transform.position, disXZ);
			Vector3 target = Vector3.Cross(_target.transform.position, disXZ);

			float distance = Vector3.Distance(start, target);
			_distanceText.text = $"{distance:N2}M";
		}

		#endregion

		#region Protected Method
		#endregion
	}
}
