namespace FisherMarket.Contracts
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IFishRepository Fish { get; }
        void Save();
    }
}
