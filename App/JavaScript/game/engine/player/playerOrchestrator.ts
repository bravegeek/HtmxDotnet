import { FlatVec } from '../physics/vector';
import { Stage } from '../stage/stageComponents';
import { World } from '../world/world';
import {
  ECBComponent,
  JumpComponent,
  PlayerFlagsComponent,
  PositionComponent,
  SpeedsComponent,
  SpeedsComponentBuilder,
  VelocityComponent,
} from './playerComponents';

type speedBuilderOptions = (scb: SpeedsComponentBuilder) => void;

const defaultSpeedsBuilderOptions: speedBuilderOptions = (
  scb: SpeedsComponentBuilder
) => {
  scb.SetWalkSpeeds(12, 2);
  scb.SetRunSpeeds(20, 2.5);
  scb.SetFallSpeeds(10, 15);
  scb.SetAerialSpeeds(0.8, 18);
  scb.SetGroundedVelocityDecay(0.8);
};

export class Player {
  private _world?: World;

  private readonly _Position: PositionComponent;
  private readonly _Velocity: VelocityComponent;
  private readonly _Flags: PlayerFlagsComponent;
  private readonly _Speeds: SpeedsComponent;
  private readonly _ECB: ECBComponent;
  private readonly _Jump: JumpComponent;

  constructor(sbo: speedBuilderOptions = defaultSpeedsBuilderOptions) {
    const speedsBuilder = new SpeedsComponentBuilder();
    sbo(speedsBuilder);

    this._Position = new PositionComponent();
    this._Velocity = new VelocityComponent();
    this._Speeds = speedsBuilder.Build();
    this._Flags = new PlayerFlagsComponent();
    this._ECB = new ECBComponent();
    this._Jump = new JumpComponent(20, 2);
  }

  public SetWorld(world: World) {
    this._world = world;
  }

  // This method is for inputs from the player
  public AddXImpulse(impulse: number): void {
    if (!this.IsGrounded()) {
      this._Velocity.AddClampedXImpulse(
        impulse,
        this._Speeds.AerialSpeedInpulseLimit
      );
      return;
    }

    if (this._Flags.IsRunning()) {
      this._Velocity.AddClampedXImpulse(this._Speeds.MaxRunSpeed, impulse);
      return;
    }

    if (this._Flags.IsWakling()) {
      this._Velocity.AddClampedXImpulse(this._Speeds.MaxWalkSpeed, impulse);
    }
  }

  // This method is for inputs from the player
  public AddYImpulse(impulse: number): void {
    if (this._Flags.IsFastFalling()) {
      this._Velocity.AddClampedYImpulse(impulse, this._Speeds.FastFallSpeed);
      return;
    }

    this._Velocity.AddClampedYImpulse(impulse, this._Speeds.FallSpeed);
  }

  public SetXVelocity(vx: number): void {
    this._Velocity.Vel.X = vx;
  }

  public SetYVelocity(vy: number): void {
    this._Velocity.Vel.Y = vy;
  }

  public SetPlayerPostion(x: number, y: number): void {
    this._Position.Pos.X = x;
    this._Position.Pos.Y = y;
    this._ECB.MoveToPosition(x, y);
  }

  public IsGrounded(): boolean {
    const grnd = this._world!.stage!.StageVerticies.GetGround();
    const grndLength = grnd.length - 1;
    for (let i = 0; i < grndLength; i++) {
      const va = grnd[i];
      const vb = grnd[i + 1];
      if (this._ECB.DetectGroundCollision(va, vb)) {
        return true;
      }
    }
    return false;
  }

  public get ECBVerts(): FlatVec[] {
    return this._ECB.GetVerts();
  }

  public get Postion(): FlatVec {
    return this._Position.Pos;
  }

  public get Velocity(): FlatVec {
    return this._Velocity.Vel;
  }

  public get FallSpeed(): number {
    return this._Speeds.FallSpeed;
  }

  public get GroundedVelocityDecay(): number {
    return this._Speeds.GroundedVelocityDecay;
  }

  public get AerialVelocityDecay(): number {
    return this._Speeds.AerialVelocityDecay;
  }
}
