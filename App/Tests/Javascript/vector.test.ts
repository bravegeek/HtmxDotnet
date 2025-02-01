import {
  FlatVec,
  SET_POOL,
  VectorAdder,
  VectorResultAllocator,
  ZERO_POOL,
} from '../../JavaScript/game/engine/physics/vector';
import { VecResultPool } from '../../JavaScript/game/pools/VecResultPool';

test('vec library pool test', () => {
  const vrp = new VecResultPool(10);

  SET_POOL(vrp);

  const vt1 = VectorResultAllocator(1, 2);
  const vt2 = VectorResultAllocator(3, 4);

  const update = new FlatVec(0, 0);

  const result = VectorAdder(vt1, vt2);

  update.X = result.GetX();
  update.Y = result.GetY();

  expect(result.GetX()).toBe(4);
  expect(result.GetY()).toBe(6);

  ZERO_POOL();

  expect(result.GetX()).toBe(0);
  expect(result.GetY()).toBe(0);
});
