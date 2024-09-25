import networkx as nx
import matplotlib.pyplot as plt

def draw_deadlock_graph():
    G = nx.DiGraph()
    
    # Agregamos nodos (Procesos y Recursos)
    G.add_nodes_from(["Proceso 1", "Proceso 2", "Recurso 1", "Recurso 2"])
    
    # Agregamos aristas (Dependencias entre procesos y recursos)
    G.add_edges_from([("Proceso 1", "Recurso 1"), ("Recurso 1", "Proceso 2"),
                      ("Proceso 2", "Recurso 2"), ("Recurso 2", "Proceso 1")])
    
    pos = nx.spring_layout(G)
    nx.draw(G, pos, with_labels=True, node_color='lightblue', node_size=2000, font_size=12, font_weight='bold', arrows=True)
    plt.title("Ciclo de Interbloqueo")
    plt.show()

draw_deadlock_graph()
