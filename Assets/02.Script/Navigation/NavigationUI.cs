using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace EverythingStore.UI
{
	public class NavigationUI : MonoBehaviour
	{
		#region Field
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Transform _player;
		[SerializeField] private RectTransform _arrowIcon;
		[SerializeField] private RectTransform _dotIcon;
		[SerializeField] private Vector2 _dotRange;
		[SerializeField] private TMP_Text _distanceText;

		private Camera _mainCam;
		private RectTransform _rectTransfrom;
		private int _width;
		private int _height;
		private Image _image;
		private Transform _target;
		#endregion

		#region Property
		#endregion

		#region Event
		#endregion

		#region UnityCycle
		private void Awake()
		{
			_rectTransfrom = GetComponent<RectTransform>();
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
			Vector2 screenPos = WorldToScreenPos(_target.position);

			//���� �ۿ� �ִ� ��� Dot �׺���̼� ���
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
			//ȭ��ǥ ������ ������̼� ���
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
			_canvas.enabled = true;
			_target = target;
			gameObject.SetActive(true);
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
			float distance = Vector3.Distance(_player.transform.position, _target.transform.position);
			_distanceText.text = $"{distance:N2}M";
		}
		#endregion

		#region Protected Method
		#endregion









	}
}