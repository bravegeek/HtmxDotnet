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

export const VectorToVectorResultAllocator = (fv: FlatVec) => {
  return VectorResultAllocator(fv.X, fv.Y);
};

export function LineSegmentIntersection(
  x1: number,
  y1: number,
  x2: number,
  y2: number,
  x3: number,
  y3: number,
  x4: number,
  y4: number
): boolean {
  const denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
  const numeA = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
  const numeB = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);

  if (denom === 0) {
    return false;
  }

  const uA = numeA / denom;
  const uB = numeB / denom;

  if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1) {
    return true;
  }

  return false;
}

function AlternateLineSegmentIntersection(
  x1: number,
  y1: number,
  x2: number,
  y2: number,
  x3: number,
  y3: number,
  x4: number,
  y4: number
): LineSegmentIntersectionResult {
  const denom = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
  const numeA = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
  const numeB = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);

  if (denom === 0) {
    return LineSegmentIntersectionResult.False();
  }

  const uA = numeA / denom;
  const uB = numeB / denom;

  if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1) {
    //return true;
    return LineSegmentIntersectionResult.True(
      VectorResultAllocator(x1 + uA * (x2 - x1), y1 + uA * (y2 - y1))
    );
  }

  return LineSegmentIntersectionResult.False();
}

class LineSegmentIntersectionResult {
  private Success: boolean;
  private VecResult: IVecResultDto | undefined;

  private constructor(
    flag: boolean,
    vecRes: IVecResultDto | undefined = undefined
  ) {
    this.Success = flag;
    this.VecResult = vecRes;
  }

  static True(vecRes: IVecResultDto): LineSegmentIntersectionResult {
    return new LineSegmentIntersectionResult(true, vecRes);
  }

  static False(): LineSegmentIntersectionResult {
    return new LineSegmentIntersectionResult(false);
  }
}
