namespace EverythingStore.Optimization
{
	public interface IPoolObject_GetInitialization
	{
		/// <summary>
		/// Pool에서 빼올 때 호출됩니다.
		/// </summary>
		void GetPoolObjectInitialization();
	}
}