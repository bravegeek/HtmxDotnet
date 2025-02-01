import { IVecResultDto, VecResultPool } from '../../pools/VecResultPool';

let vrp = new VecResultPool(1000);

export class FlatVec {
  X: number;
  Y: number;

  constructor(x: number, y: number) {
    this.X = x;
    this.Y = y;
  }
}

export function SET_POOL(pool: VecResultPool) {
  vrp = pool;
}

export function ZERO_POOL() {
  vrp.Zero();
}

export const VectorAdder = (
  v1: IVecResultDto,
  v2: IVecResultDto
): IVecResultDto => {
  return VectorResultAllocator(v1.GetX() + v2.GetX(), v1.GetY() + v2.GetY());
};

export const VectorSubtractor = (
  v1: IVecResultDto,
  v2: IVecResultDto
): IVecResultDto => {
  return VectorResultAllocator(v1.GetX() - v2.GetX(), v1.GetY() - v2.GetY());
};

export const VectorMultiplier = (
  v: IVecResultDto,
  s: number
): IVecResultDto => {
  return VectorResultAllocator(v.GetX() * s, v.GetY() * s);
};

export const VectorNegator = (v: IVecResultDto): IVecResultDto => {
  return VectorResultAllocator(-v.GetX(), -v.GetY());
};

export const VectorDivider = (v: IVecResultDto, s: number): IVecResultDto => {
  return VectorResultAllocator(v.GetX() / s, v.GetY() / s);
};

export const Length = (v: IVecResultDto): number => {
  return Math.sqrt(v.GetX() * v.GetX() + v.GetY() * v.GetY());
};

export const Distance = (v1: IVecResultDto, v2: IVecResultDto): number => {
  const dx = v1.GetX() - v2.GetX();
  const dy = v1.GetY() - v2.GetY();
  return Math.sqrt(dx * dx + dy * dy);
};

export const Normalize = (v: IVecResultDto): IVecResultDto => {
  const length = Length(v);
  return VectorResultAllocator(v.GetX() / length, v.GetY() / length);
};

export const DotProduct = (v1: IVecResultDto, v2: IVecResultDto): number => {
  return v1.GetX() * v2.GetX() + v1.GetY() * v2.GetY();
};

export const CrossProduct = (v1: IVecResultDto, v2: IVecResultDto): number => {
  return v1.GetX() * v2.GetY() - v1.GetY() * v2.GetX();
};

export const VectorResultAllocator = (
  x: number = 0,
  y: number = 0
): IVecResultDto => {
  let VecResultDto = vrp.Rent();
  VecResultDto._setXY(x, y);
  return VecResultDto;
};
