using System.Collections.Generic;

namespace SpaceInvadersGame.Entity
{
  public interface IEntityContainer
  {
    void AddEntity(EntityBase entity);
    IEnumerable<EntityBase> Entities { get; }

    bool IsFree(double x, double y, int width, int height);
  }
}
