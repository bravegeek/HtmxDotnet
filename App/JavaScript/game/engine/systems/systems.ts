import { IntersectsPolygons } from '../physics/collisions';
import {
  VectorAdder,
  VectorMultiplier,
  VectorNegator,
  VectorToVectorResultAllocator,
} from '../physics/vector';
import { Player } from '../player/playerOrchestrator';
import { StageOrchestrator } from '../stage/stageComponents';

const correctionDepth: number = 0.01;

export function StageCollisionDetection(
  p: Player,
  s: StageOrchestrator
): boolean {
  const stageVerts = s.StageVerticies.GetVerts();
  const playerVerts = p.GetVerts();

  // detect the collision
  const collisionResult = IntersectsPolygons(playerVerts, stageVerts);

  if (collisionResult.collision) {
    const move = VectorMultiplier(
      VectorNegator(collisionResult.normal),
      collisionResult.depth
    );

    const normalX = collisionResult.normal.GetX();
    const normalY = collisionResult.normal.GetY();
    const playerPos = p.GetPostion();
    const playerPosDTO = VectorToVectorResultAllocator(playerPos);

    //Ground correction
    if (normalX == 0 && normalY > 0) {
      //add the correction depth
      move._addToY(correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.GetX(), resolution.GetY());
      p.Flags.Ground();
      return true;
    }

    //Right wall correction
    if (normalX > 0 && normalY == 0) {
      move._addToX(correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.GetX(), resolution.GetY());
      if (!p.Flags.IsGrounded()) {
        p.Flags.SetRightWallRiddingTrue();
        return true;
      }
    }

    // Left Wall Correction
    if (normalX < 0 && normalY == 0) {
      move._addToX(-correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.GetX(), resolution.GetY());
      if (!p.Flags.IsGrounded()) {
        p.Flags.SetLeftWallRiddingTrue();
        return true;
      }
    }

    //ceiling
    if (normalX == 0 && normalY < 0) {
      move._addToY(-correctionDepth);
      let resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.GetX(), resolution.GetY());
      return true;
    }

    // corner case

    if (Math.abs(normalX) > 0 && Math.abs(normalY) > 0) {
      const fix = move.GetX() <= 0 ? move.GetY() : -move.GetY();
      move._addToX(fix);
      move._setY(0);
      const resolution = VectorAdder(playerPosDTO, move);
      p.SetPlayerPostion(resolution.GetX(), resolution.GetY());
      return true;
    }
    return false;
  }

  return false;
}

function GroundCOllision() {}
