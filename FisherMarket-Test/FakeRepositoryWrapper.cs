using FisherMarket.Contracts;
using FisherMarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FisherMarket_Test
{
    public class FakeRepositoryWrapper : IRepositoryWrapper
    {
        private IFishRepository _fish;
        public FakeRepositoryWrapper()
        {
        }

        public IUserRepository User => throw new NotImplementedException();

        public IRoleRepository Role => throw new NotImplementedException();

        public IFishRepository Fish
        {
            get
            {
                if (_fish == null)
                {
                    _fish = new FishRepositoryFake();
                }

                return _fish;
            }
        
        }

        public void Save()
        {
        }
    }
    public class FishRepositoryFake : IFishRepository
    {
        private readonly List<Fish> _fishList;
        public FishRepositoryFake()
        {
            _fishList = new List<Fish>() {
                new Fish() {Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200").GetHashCode(),
                    Name = "Fish1", Definition = "Definition1", Price = 101.0},
                new Fish() {Id = new Guid("815accac-fd5b-478a-a9d6-f171a2f6ae7f").GetHashCode(),
                    Name = "Fish2", Definition = "Definition2", Price = 102.0},
                new Fish() {Id = new Guid("33704c4a-5b87-464c-bfb6-51971b4d18ad").GetHashCode(),
                    Name = "Fish3", Definition = "Definition3", Price = 103.0}
            };
        }
        public new Fish Create(Fish newFish)
        {
            newFish.Id = Guid.NewGuid().GetHashCode();
            _fishList.Add(newFish);
            return newFish;
        }

        public new void Delete(Fish entity)
        {
            _fishList.Remove(entity);
        }

        public new IQueryable<Fish> FindAll()
        {
            return _fishList.AsQueryable();
        }

        public new IQueryable<Fish> FindByCondition(Expression<Func<Fish, bool>> expression)
        {
            return _fishList.AsQueryable().Where(expression);
        }

        public new void Update(Fish entity)
        {
            var existing = _fishList.AsQueryable().Where(f => f.Id == entity.Id).FirstOrDefault();
            existing = entity;
        }
    }
}
