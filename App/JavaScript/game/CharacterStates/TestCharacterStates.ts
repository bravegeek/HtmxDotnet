import { Player } from '../engine/player/playerOrchestrator';
import { STATES } from '../FSM/FiniteState';
import { FSMState } from '../FSM/FiniteStateMachine';
import { InputAction } from '../loops/Input';

const Idle = {
  StateName: 'IDLE',
  StateId: STATES.IDLE,
} as FSMState;

const StartWalk: FSMState = {
  StateName: 'START_WALK',
  StateId: STATES.START_WALK,
  FrameLength: 5,
  OnEnter: (p: Player) => {
    console.log('Start Walk');
  },
  OnUpdate: (p: Player, ia: InputAction) => {
    if (ia.LXAxsis > 0) {
      p.FaceRight;
    }
    if (ia.LXAxsis < 0) {
      p.FaceLeft;
    }
    p.AddWalkImpulse(ia.LXAxsis);
  },
  OnExit: (p: Player) => {
    console.log('Exit Start Walk');
  },
};

const Walk: FSMState = {
  StateName: 'WALK',
  StateId: STATES.WALK,
  OnEnter: (p: Player) => {
    console.log('Walk');
  },
  OnUpdate: (p: Player, ia: InputAction) => {
    if (ia.LXAxsis > 0) {
      p.FaceRight;
    }
    if (ia.LXAxsis < 0) {
      p.FaceLeft;
    }
    p.AddWalkImpulse(ia.LXAxsis);
  },
  OnExit: (p: Player) => {
    console.log('Exit Walk');
  },
};

const Trun: FSMState = {
  StateName: 'TURN',
  StateId: STATES.TURN,
  OnEnter: (p: Player) => {
    console.log('Turn');
  },
  OnExit: (p: Player) => {
    p.ChangeDirections();
  },
};

const JumpSquat: FSMState = {
  StateName: 'JUMPSQUAT',
  StateId: STATES.JUMPSQUAT,
  FrameLength: 4,
  OnEnter: (p: Player) => {
    console.log('Jump Squat');
  },
  OnExit: (p: Player) => {
    console.log('Exit Jump Squat');
  },
};

const Jump: FSMState = {
  StateName: 'JUMP',
  StateId: STATES.JUMP,
  OnEnter: (p: Player) => {
    p.AddToPlayerYPosition(-0.5);
    p.AddJumpImpulse();
    console.log('Jump');
  },
  OnExit: (p: Player) => {
    console.log('Exit Jump');
  },
};
