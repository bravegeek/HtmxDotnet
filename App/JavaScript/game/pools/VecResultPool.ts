export class VecResultPool {
  private pool: Array<VecResultDto>;
  private poolIndex: number = 0;

  constructor(poolSize: number) {
    this.pool = new Array(poolSize);

    for (let i = 0; i < poolSize; i++) {
      this.pool[i] = new VecResultDto();
    }
  }

  Rent(): VecResultDto {
    let pi = this.poolIndex;
    let p = this.pool;
    let pLength = p.length;

    if (pi < pLength) {
      this.poolIndex++;
      return p[pi];
    }

    return new VecResultDto();
  }

  Zero(): void {
    let p = this.pool;
    let pi = this.poolIndex;

    this.poolIndex = 0;

    for (let i = 0; i < pi; i++) {
      p[i]._zero();
    }
  }
}

export interface IVecResultDto {
  GetX(): number;
  GetY(): number;
  _setXY(x: number, y: number): void;
  _zero(): void;
}

class VecResultDto implements IVecResultDto {
  private ResX: number;
  private ResY: number;

  constructor(x: number = 0, y: number = 0) {
    this.ResX = x;
    this.ResY = y;
  }

  GetX() {
    return this.ResX;
  }

  GetY() {
    return this.ResY;
  }

  _setXY(x: number, y: number) {
    this.ResX = x;
    this.ResY = y;
  }

  _zero() {
    this._setXY(0, 0);
  }
}
