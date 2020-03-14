import React from 'react';
import axios from 'axios';
import { Header, Icon, List } from 'semantic-ui-react';
import { Values } from './type';

interface PropsInterface {

}

interface StateInterface {
  values: Values[]
}

class App extends React.Component<PropsInterface, StateInterface> {
  constructor(props: PropsInterface) {
    super(props);
    this.state = {
      values: []
    }
  }

  componentDidMount = () => {
    axios({
      method: 'get',
      url: `${process.env.REACT_APP_BACKEND_ENDPOINT}/values`,
    }).then(({ data }) => {
      this.setState({ values: [...data] })
    });
  }
  render() {
    const { values } = this.state;
    return (
    <div>
      <Header as='h2'>
        <Icon name='users' />
        <Header.Content>Reactivity</Header.Content>
      </Header>
      <List>
        {
          values.map(({ id, name } : Values) => (
            <List.Item key={id}>{name}</List.Item>
          ))
        }
      </List>
    </div>
  );
  }
}

export default App;
