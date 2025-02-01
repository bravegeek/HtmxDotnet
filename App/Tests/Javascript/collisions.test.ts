import { IntersectsPolygons } from '../../JavaScript/game/engine/physics/collisions';
import {
  FlatVec,
  VectorAdder,
  VectorResultAllocator,
  ZERO_POOL,
} from '../../JavaScript/game/engine/physics/vector';

let poly1: Array<FlatVec>;
let poly2: Array<FlatVec>;

beforeEach(() => {
  poly1 = new Array<FlatVec>();
  poly2 = new Array<FlatVec>();

  poly1[0] = new FlatVec(0, 0);
  poly1[1] = new FlatVec(50, 0);
  poly1[2] = new FlatVec(50, 50);
  poly1[3] = new FlatVec(0, 50);

  poly2[0] = new FlatVec(0, 0);
  poly2[1] = new FlatVec(50, 0);
  poly2[2] = new FlatVec(50, 50);
  poly2[3] = new FlatVec(0, 50);

  ZERO_POOL();
});

test('Test Move', () => {
  let p1 = Move(poly1, new FlatVec(100, 0));

  expect(p1[0].X).toBe(100);
  expect(p1[1].X).toBe(150);
});

test('IntersectsPolygons returns false', () => {
  let p1 = Move(poly1, new FlatVec(100, 100));
  let p2 = Move(poly2, new FlatVec(300, 300));

  let res = IntersectsPolygons(p1, p2);

  expect(res.collision).toBeFalsy();
});

test('IntersectsPolygons returns true', () => {
  let p1 = Move(poly1, new FlatVec(100, 100));
  let p2 = Move(poly2, new FlatVec(110, 100));

  let res = IntersectsPolygons(p1, p2);

  expect(res.collision).toBeTruthy();
});

function Move(poly: Array<FlatVec>, pos: FlatVec) {
  poly[0] = pos;
  let posDto = VectorResultAllocator(pos.X, pos.Y);
  var dto = VectorResultAllocator();

  for (let i = 1; i < poly.length; i++) {
    let vert = poly[i];
    dto._setXY(vert.X, vert.Y);
    let res = VectorAdder(dto, posDto);
    vert.X = res.GetX();
    vert.Y = res.GetY();
  }

  return poly;
}
