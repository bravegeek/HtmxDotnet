import { VectorResultAllocator } from '../engine/physics/vector';
import { IVecResultDto } from './VecResultPool';

export class CollisionResultPool {
  private pool: Array<CollisionResult>;
  private poolIndex: number = 0;

  constructor(poolSize: number) {
    this.pool = new Array(poolSize);

    for (let i = 0; i < poolSize; i++) {
      this.pool[i] = new CollisionResult();
    }
  }

  Rent(): CollisionResult {
    let pi = this.poolIndex;
    let p = this.pool;
    let pLength = p.length;

    if (pi < pLength) {
      this.poolIndex++;
      return p[pi];
    }

    return new CollisionResult();
  }

  zero(): void {
    let p = this.pool;
    let pi = this.poolIndex;

    this.poolIndex = 0;

    for (let i = 0; i < pi; i++) {
      p[i]._setCollisionFalse();
    }
  }
}

export interface ICollisionResult {
  _setCollisionTrue(normal: IVecResultDto, depth: number): void;
  _setCollisionFalse(): void;
  get collision(): boolean;
  get depth(): number;
}

class CollisionResult implements ICollisionResult {
  private _collision: boolean = false;
  readonly normal: IVecResultDto = VectorResultAllocator();
  private _depth: number = 0;

  _setCollisionTrue(normal: IVecResultDto, depth: number): void {
    this._collision = true;
    this.normal._setXY(normal.GetX(), normal.GetY());
    this._depth = depth;
  }

  _setCollisionFalse(): void {
    this._collision = false;
    this.normal._setXY(0, 0);
    this._depth = 0;
  }

  public get collision(): boolean {
    return this._collision;
  }

  public get depth(): number {
    return this._depth;
  }
}
