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
  _setX(x: number): void;
  _addToX(x: number): void;
  _addToY(y: number): void;
  _setY(y: number): void;
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
  _addToX(x: number): void {
    this.ResX += x;
  }
  _addToY(y: number): void {
    this.ResY += y;
  }

  GetX(): number {
    return this.ResX;
  }

  GetY(): number {
    return this.ResY;
  }

  _setX(x: number): void {
    this.ResX = x;
  }

  _setY(y: number): void {
    this.ResY = y;
  }

  _setXY(x: number, y: number): void {
    this.ResX = x;
    this.ResY = y;
  }

  _zero(): void {
    this._setXY(0, 0);
  }
}
