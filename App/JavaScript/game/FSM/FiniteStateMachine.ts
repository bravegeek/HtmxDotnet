import { Player } from '../engine/player/playerOrchestrator';
import { InputAction } from '../loops/Input';

export type FSMState = {
  StateName: string;
  StateId: number;
  FrameLength?: number;
  InteruptFrame?: number;
  OnEnter?: (p: Player) => void;
  OnUpdate?: (p: Player, inputAction: InputAction) => void;
  OnExit?: (p: Player) => void;
};
