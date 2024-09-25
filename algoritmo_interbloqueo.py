import networkx as nx
import matplotlib.pyplot as plt

def detect_deadlock(G):
    try:
        # Intentamos detectar un ciclo en el grafo
        cycle = nx.find_cycle(G, orientation='original')
        print("Interbloqueo detectado: ", cycle)
    except nx.NetworkXNoCycle:
        print("No se ha detectado un interbloqueo")

# Crear el grafo del interbloqueo
G = nx.DiGraph()
G.add_edges_from([("Proceso 1", "Recurso 1"), ("Recurso 1", "Proceso 2"),
                  ("Proceso 2", "Recurso 2"), ("Recurso 2", "Proceso 1")])

detect_deadlock(G)
