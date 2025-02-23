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
};

export class Player {
  private _world?: World;

  private readonly Position: PositionComponent;
  private readonly Velocity: VelocityComponent;
  private readonly Flags: PlayerFlagsComponent;
  private readonly Speeds: SpeedsComponent;
  private readonly ECB: ECBComponent;
  private readonly Jump: JumpComponent;

  constructor(sbo: speedBuilderOptions = defaultSpeedsBuilderOptions) {
    const speedsBuilder = new SpeedsComponentBuilder();
    sbo(speedsBuilder);

    this.Position = new PositionComponent();
    this.Velocity = new VelocityComponent();
    this.Speeds = speedsBuilder.Build();
    this.Flags = new PlayerFlagsComponent();
    this.ECB = new ECBComponent();
    this.Jump = new JumpComponent(20, 2);
  }

  public SetWorld(world: World) {
    this._world = world;
  }

  // This method is for inputs from the player
  public AddXImpulse(impulse: number): void {
    if (!this.IsGrounded()) {
      this.Velocity.AddClampedXImpulse(
        impulse,
        this.Speeds.AerialSpeedInpulseLimit
      );
      return;
    }

    if (this.Flags.IsRunning()) {
      this.Velocity.AddClampedXImpulse(this.Speeds.MaxRunSpeed, impulse);
      return;
    }

    if (this.Flags.IsWakling()) {
      this.Velocity.AddClampedXImpulse(this.Speeds.MaxWalkSpeed, impulse);
    }
  }

  // This method is for inputs from the player
  public AddYImpulse(impulse: number): void {
    if (this.Flags.IsFastFalling()) {
      this.Velocity.AddClampedYImpulse(impulse, this.Speeds.FastFallSpeed);
      return;
    }

    this.Velocity.AddClampedYImpulse(impulse, this.Speeds.FallSpeed);
  }

  public SetXVelocity(vx: number): void {
    this.Velocity.Vel.X = vx;
  }

  public SetYVelocity(vy: number): void {
    this.Velocity.Vel.Y = vy;
  }

  public IsGrounded(): boolean {
    const grnd = this._world!.stage!.StageVerticies.GetGround();
    const grndLength = grnd.length - 1;
    for (let i = 0; i < grndLength; i++) {
      const va = grnd[i];
      const vb = grnd[i + 1];
      if (this.ECB.DetectGroundCollision(va, vb)) {
        return true;
      }
    }
    return false;
  }

  public GetVerts(): FlatVec[] {
    return this.ECB.GetVerts();
  }

  public GetPostion(): FlatVec {
    return this.Position.Pos;
  }

  public SetPlayerPostion(x: number, y: number): void {
    this.Position.Pos.X = x;
    this.Position.Pos.Y = y;
    this.ECB.MoveToPosition(x, y);
  }

  public GetECB(): ECBComponent {
    return this.ECB;
  }
}
