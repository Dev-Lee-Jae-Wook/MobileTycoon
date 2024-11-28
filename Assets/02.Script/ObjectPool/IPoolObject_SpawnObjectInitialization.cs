namespace EverythingStore.Optimization
{
	public interface IPoolObject_SpawnObjectInitialization
	{
		/// <summary>
		/// 내부에서 생성이 필요할 때 호출합니다.
		/// </summary>
		/// <param name="manger"></param>
		void SpawnObjectInitialization(ObjectPoolManger manger);
	}
}