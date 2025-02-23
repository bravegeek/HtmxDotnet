import { IntersectsPolygons } from '../physics/collisions';
import {
  VectorAdder,
  VectorMultiplier,
  VectorNegator,
  VectorResultAllocator,
  VectorToVectorResultAllocator,
} from '../physics/vector';
import { Player } from '../player/playerOrchestrator';
import { Stage } from '../stage/stageComponents';

const correctionDepth: number = 0.01;

export function StageCollisionDetection(p: Player, s: Stage): boolean {
  const stageVerts = s.StageVerticies.GetVerts();
  const playerVerts = p.GetVerts();

  // detect the collision
  const collisionResult = IntersectsPolygons(playerVerts, stageVerts);

  if (collisionResult.collision) {
    let dto = VectorResultAllocator(
      collisionResult.normX,
      collisionResult.normY
    );
    const move = VectorMultiplier(VectorNegator(dto), collisionResult.depth);

    const normalX = collisionResult.normX;
    const normalY = collisionResult.normY;
    const playerPos = p.GetPostion();
    const playerPosDTO = VectorToVectorResultAllocator(playerPos);

    //Ground correction
    if (normalX == 0 && normalY > 0) {
      //add the correction depth
      move.AddToY(correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.X, resolution.Y);
      //p.Flags.Ground();
      return true;
    }

    //Right wall correction
    if (normalX > 0 && normalY == 0) {
      move.AddToX(correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.X, resolution.Y);
      return true;
      // if (!p.Flags.IsGrounded()) {
      //   p.Flags.SetRightWallRiddingTrue();
      //   return true;
      // }
    }

    // Left Wall Correction
    if (normalX < 0 && normalY == 0) {
      move.AddToX(-correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.X, resolution.Y);
      return true;
      // if (!p.Flags.IsGrounded()) {
      //   p.Flags.SetLeftWallRiddingTrue();
      //   return true;
      // }
    }

    //ceiling
    if (normalX == 0 && normalY < 0) {
      move.AddToY(-correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.X, resolution.Y);
      return true;
    }

    // corner case

    if (Math.abs(normalX) > 0 && Math.abs(normalY) > 0) {
      const fix = move.X <= 0 ? move.Y : -move.Y;
      move.AddToX(fix);
      move._setY(0);
      const resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.X, resolution.Y);
      return true;
    }
    return false;
  }

  return false;
}

function GroundCOllision() {}
