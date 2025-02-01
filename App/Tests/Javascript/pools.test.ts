import { VecResultPool } from '../../JavaScript/game/pools/VecResultPool';

test('Pools Test 1', () => {
  const SUT = new VecResultPool(1000);
  const result = SUT.Rent();

  expect(result.GetX()).toBe(0);
  expect(result.GetY()).toBe(0);

  result._setXY(1, 2);

  expect(result.GetX()).toBe(1);
  expect(result.GetY()).toBe(2);

  SUT.Zero();

  expect(result.GetX()).toBe(0);
  expect(result.GetY()).toBe(0);
});
