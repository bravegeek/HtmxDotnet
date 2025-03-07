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

export interface IVecResult {
  get X(): number;
  get Y(): number;
  AddToX(x: number): void;
  AddToY(y: number): void;
  _setX(x: number): void;
  _setY(y: number): void;
  _setXY(x: number, y: number): void;
  _zero(): void;
}

class VecResultDto implements IVecResult {
  private _x: number;
  private _y: number;

  constructor(x: number = 0, y: number = 0) {
    this._x = x;
    this._y = y;
  }

  AddToX(x: number): void {
    this._x += x;
  }

  AddToY(y: number): void {
    this._y += y;
  }

  get X(): number {
    return this._x;
  }

  get Y(): number {
    return this._y;
  }

  _setX(x: number): void {
    this._x = x;
  }

  _setY(y: number): void {
    this._y = y;
  }

  _setXY(x: number, y: number): void {
    this._x = x;
    this._y = y;
  }

  _zero(): void {
    this._setXY(0, 0);
  }
}
